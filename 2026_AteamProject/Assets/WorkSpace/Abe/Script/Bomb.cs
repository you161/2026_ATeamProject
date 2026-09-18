using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;

public class Bomb : MonoBehaviour
{
    [Header("スクリプト")]
    [SerializeField] private SEManager seManager = null;
    [SerializeField] private PlayerKnockback playerKnokback = null;

    [Header("投擲設定")]
    [SerializeField] private float throwDistance = 10.0f;
    [SerializeField] private float gravityPower = 5.0f;
    [SerializeField] private float throwUpPower = 5.0f;
    [SerializeField] private float maxGround = -10.0f;

    [Header("爆発設定")]
    [SerializeField] private float explosionTime = 5.0f;
    [SerializeField] private float explosionSize = 2.0f;
    [SerializeField] private float tTime = 0.01f;

    [Header("クールダウン設定")]
    [SerializeField] private Image gaugeImage = null;
    [SerializeField] private float cooldownTime = 0.0f;

    [Header("爆弾")]
    [SerializeField] private GameObject bombObject = null;

    [Header("爆風エフェクト")]
    [SerializeField] private GameObject bombEffect = null;

    [Header("軌道")]
    [SerializeField] private LineRenderer lineRenderer = null;
    [SerializeField] private float timeStep = 0.05f;
    [SerializeField] private float maxPredictionTime = 5.0f;

    [Header("衝突判定")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject omenObject = null;

    [Header("コントローラ入力")]
    [SerializeField] private PlayerControllerInput playerControllerInput = null;

    [SerializeField] private PlayerJump playerJump = null;

    private bool isThrow = false;
    private bool isReady = false;
    private bool isExplosion = false;
    private bool isCoolDown = false;

    public bool IsReady { get => isReady; }
    public bool IsThrow { get => isThrow; }
    public bool IsExplosion { get => isExplosion; }

    private float throwTime = 0.0f;
    private float countTime = 0.0f;

    private CountDown countDown = null;
    private RaundManager raundManager = null;

    private GameObject currentBomb = null;
    private BombCollider currentBombScript = null;
    private GameObject currentBombCapsule = null;

    private Color baseColor = new Color(1.0f, 0.0f, 0.0f);
    private Color downColor = new Color(0.5f, 0.0f, 0.0f);

    // ゲージの初めの高さ
    private float gaugeHeight = 0.0f;

    //投げ始めた位置
    private Vector3 throwStartPosition;

    //投げる瞬間の初速度
    private Vector3 throwVelocity;

    private void Start()
    {
        countDown = GameObject.Find("CountDownManager").GetComponent<CountDown>();
        raundManager = GameObject.Find("RaundManager").GetComponent<RaundManager>();
        omenObject.SetActive(false);
        gaugeHeight = gaugeImage.rectTransform.position.y;
    }
    private void Update()
    {
        Vector3 gaugePos = gaugeImage.rectTransform.position;
        gaugePos = transform.position;

        if (gaugePos.y > gaugeHeight)
        {
            gaugePos.y = gaugeHeight;
        }
        gaugeImage.rectTransform.position = gaugePos;

        // ラウンド更新 強制リセット
        if (raundManager.IsRoundOver)
        {
            isReady = false;
            isThrow = false;
            isExplosion = false;
            isCoolDown = false;
            gaugeImage.fillAmount = 1;
            gaugeImage.color = baseColor;
            lineRenderer.positionCount = 0;
            omenObject.SetActive(false);
            if (currentBomb != null)
            {
                Destroy(currentBomb.gameObject);
            }
            currentBomb = null;
            return;
        }
        if (countDown != null && countDown.IsPlayCount && !countDown.IsGo)
        {
            return;
        }

        if (!isReady)
        {

            //Space1回目
            if (!isCoolDown)
            {
                if (!playerJump.IsFrontJumping && playerControllerInput.EastButtonPressed)
                {
                    CreateBomb();
                    isReady = true;
                    seManager.BombPickupSE();
                    countTime = 0.0f;
                }
            }
        }
        else
        {
            //爆弾を持っている状態
            currentBomb.transform.position = transform.position + Vector3.up * 0.3f + Vector3.fwd * 0.3f;
            //Space2回目
            if (!isThrow && playerControllerInput.EastButtonPressed)
            {
                StartThrow();
                isCoolDown = true;

                gaugeImage.color = downColor;
                gaugeImage.fillAmount = 0.0f;
            }
        }

        //軌道表示
        if (isReady && !isThrow)
        {
            DrawTrajectory();
            CountDownExplosion();
        }

        //投げる処理
        if (isThrow && currentBomb != null)
        {
            ThrowBomb();
        }

        // クールダウン処理
        if (isCoolDown)
        {
            gaugeImage.fillAmount += 1.0f / cooldownTime * Time.deltaTime;

            if (gaugeImage.fillAmount >= 1.0f)
            {
                isCoolDown = false;
                gaugeImage.color = baseColor;
            }
        }

        //爆発する処理
        if (isExplosion)
        {
            countTime += Time.deltaTime;

            if (countTime > tTime)
            {
                Instantiate(bombEffect, currentBomb.transform.position, Quaternion.identity);

                seManager.BombExplodeSE();

                Destroy(currentBomb.gameObject);
                currentBomb = null;
                isExplosion = false;
            }
        }
    }

    private void CreateBomb()
    {
        currentBomb = Instantiate(
            bombObject,
            transform.position + Vector3.up * 2.0f,
            bombObject.transform.rotation
        );
        currentBombScript = currentBomb.GetComponent<BombCollider>();
        currentBombScript.Getbomb(GetComponent<Bomb>());
        currentBombCapsule = currentBomb.transform.GetChild(1).gameObject;
        currentBombCapsule.SetActive(false);
    }

    private void StartThrow()
    {
        isThrow = true;

        //軌道を消す
        lineRenderer.positionCount = 0;

        //投げ始める時間をリセット
        throwTime = 0.0f;

        //投げ始める位置を保存
        throwStartPosition = currentBomb.transform.position;

        //プレイヤーの正面方向
        Vector3 direction = transform.forward;

        //水平方向だけにする
        direction.y = 0.0f;

        if (direction.sqrMagnitude > 0.0f)
        {
            direction.Normalize();
        }

        //飛行時間
        float flightTime = (2.0f * throwUpPower) / gravityPower;

        //指定距離に到達するための水平速度
        float horizontalSpeed = throwDistance / flightTime;

        //初速度
        throwVelocity = direction * horizontalSpeed;

        //上方向の速度
        throwVelocity.y = throwUpPower;
    }

    private void ThrowBomb()
    {
        //経過時間
        throwTime += Time.deltaTime;

        //放物線上の位置を計算
        Vector3 position = CalculateTrajectoryPosition(
            throwStartPosition,
            throwVelocity,
            throwTime
        );

        //爆弾を移動
        currentBomb.transform.position = position;

        // 爆発
        if (currentBombScript != null && currentBombScript.IsFlag)
        {
            isThrow = false;
            isReady = false;
            isExplosion = true;
            currentBombCapsule.SetActive(true);
            countTime = 0.0f;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                Debug.Log(player);
                if (player == gameObject)
                {
                    continue;
                }
                PlayerKnockback playerKnokuBack = player.GetComponent<PlayerKnockback>();
                playerKnokuBack.PlayKnockback(currentBomb.transform.position);
            }
            currentBomb.transform.GetChild(0).gameObject.transform.localScale = new Vector3(explosionSize, explosionSize, explosionSize);
            omenObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        // ステージ外　不発
        if (currentBomb.transform.position.y <= maxGround)
        {
            isThrow = false;
            isReady = false;
            Destroy(currentBomb.gameObject);
            currentBomb = null;
        }
    }

    private void CountDownExplosion()
    {
        countTime += Time.deltaTime;

        if (countTime > explosionTime)
        {
            isReady = false;
            isExplosion = true;
            isCoolDown = true;
            gaugeImage.color = downColor;
            gaugeImage.fillAmount = 0.0f;
            countTime = 0.0f;
            lineRenderer.positionCount = 0;
            playerKnokback.PlayKnockback(currentBomb.transform.position);
            currentBomb.transform.GetChild(0).gameObject.transform.localScale = new Vector3(explosionSize, explosionSize, explosionSize);
            omenObject.SetActive(false);
            currentBombCapsule.SetActive(true);
        }
    }

    private void DrawTrajectory()
    {
        if (currentBomb == null)
        {
            return;
        }

        Vector3 startPosition = currentBomb.transform.position;

        //プレイヤーの正面方向
        Vector3 direction = transform.forward;

        //水平方向だけにする
        direction.y = 0.0f;

        if (direction.sqrMagnitude > 0.0f)
        {
            direction.Normalize();
        }

        //飛行時間
        float flightTime = (2.0f * throwUpPower) / gravityPower;

        //指定距離に到達するための水平速度
        float horizontalSpeed = throwDistance / flightTime;

        //初速度
        Vector3 velocity = direction * horizontalSpeed;

        //上方向の速度
        velocity.y = throwUpPower;

        //最初のポイント
        Vector3 previousPosition = startPosition;

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, previousPosition);

        float time = 0.0f;

        while (time < maxPredictionTime)
        {
            //時間を進める
            time += timeStep;

            //次のポイント
            Vector3 nextPosition = CalculateTrajectoryPosition(startPosition, velocity, time);

            //Colliderに当たったか
            if (Physics.Linecast(previousPosition, nextPosition, out RaycastHit hit, groundLayer))
            {
                //衝突地点を最後のポイントにする
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                omenObject.transform.position = hit.point;
                omenObject.transform.position += Vector3.up * 0.3f;
                omenObject.transform.GetChild(0).gameObject.SetActive(true);
                omenObject.SetActive(true);

                break;
            }

            //ポイント追加
            if(omenObject.activeSelf) omenObject.SetActive(false);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, nextPosition);
            previousPosition = nextPosition;
        }
    }

    private Vector3 CalculateTrajectoryPosition(Vector3 startPosition, Vector3 velocity, float time)
    {
        // 重力
        Vector3 gravity = Vector3.down * gravityPower;
        // 放物線
        return startPosition + velocity * time + 0.5f * gravity * time * time;
    }
}