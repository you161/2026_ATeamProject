using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bomb : MonoBehaviour
{
    [Header("投擲設定")]
    [SerializeField] private float throwDistance = 10.0f;
    [SerializeField] private float gravityPower = 5.0f;
    [SerializeField] private float throwUpPower = 5.0f;
    [SerializeField] private float maxGround = -10.0f;

    [Header("爆発設定")]
    [SerializeField] private float explosionTime = 5.0f;
    [SerializeField] private float explosionSize = 2.0f;
    [SerializeField] private float tTime = 0.01f;

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

    private bool isThrow = false;
    private bool isReady = false;
    private bool isExplosion = false;
    private bool isBlast = false;

    public bool IsReady { get => isReady; }

    private float throwTime = 0.0f;
    private float countTime = 0.0f;

    private GameObject currentBomb = null;
    private BombCollider currentBombScript = null;
    private GameObject currentBombCapsule = null;

    //投げ始めた位置
    private Vector3 throwStartPosition;

    //投げる瞬間の初速度
    private Vector3 throwVelocity;

    private void Start()
    {
        omenObject.SetActive(false);
    }
    private void Update()
    {
        if (!isReady)
        {
            //Space1回目
            if (playerControllerInput.EastButtonPressed)
            {
                CreateBomb();
                countTime = 0.0f;
                isReady = true;
            }
        }
        else
        {
            //爆弾を持っている状態
            currentBomb.transform.position = transform.position + Vector3.up * 2.0f;
            //Space2回目
            if (!isThrow && playerControllerInput.EastButtonPressed)
            {
                StartThrow();
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

        //爆発する処理
        if (isExplosion)
        {
            countTime += Time.deltaTime;

            if (countTime > tTime)
            {
                Instantiate(bombEffect, currentBomb.transform.position, Quaternion.identity);

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
            countTime = 0.0f;
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
            countTime = 0.0f;
            lineRenderer.positionCount = 0;
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