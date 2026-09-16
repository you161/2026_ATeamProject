using UnityEngine;
using UnityEngine.InputSystem;

public class test : MonoBehaviour
{
    [Header("投擲設定")]
    [SerializeField] private float throwDistance = 10.0f;
    [SerializeField] private float gravityPower = 5.0f;
    [SerializeField] private float throwUpPower = 5.0f;

    [Header("爆弾")]
    [SerializeField] private GameObject bombObject = null;

    [Header("軌道")]
    [SerializeField] private LineRenderer lineRenderer = null;
    [SerializeField] private float timeStep = 0.05f;
    [SerializeField] private float maxPredictionTime = 5.0f;

    [Header("衝突判定")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject omenObject = null;

    private bool isThrow = false;
    private bool isReady = false;

    private float throwTime = 0.0f;

    private GameObject currentBomb = null;

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
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                CreateBomb();
                isReady = true;
            }
        }
        else
        {
            //爆弾を持っている状態
            currentBomb.transform.position = transform.position + Vector3.up * 2.0f;
            //Space2回目
            if (!isThrow && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                StartThrow();
            }
        }

        //軌道表示
        if (isReady && !isThrow)
        {
            DrawTrajectory();
        }

        //投げる処理
        if (isThrow && currentBomb != null)
        {
            ThrowBomb();
        }
    }

    private void CreateBomb()
    {
        currentBomb = Instantiate(
            bombObject,
            transform.position + Vector3.up * 2.0f,
            bombObject.transform.rotation
        );
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
        float distance = Vector3.Distance(currentBomb.transform.position, omenObject.transform.position);
        if (distance < 0.1f)
        {
            isThrow = false;
            isReady = false;
            currentBomb = null;
            omenObject.SetActive(false);
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
            Vector3 nextPosition = CalculateTrajectoryPosition(startPosition,velocity,time);

            //Colliderに当たったか
            if (Physics.Linecast(previousPosition,nextPosition, out RaycastHit hit,groundLayer))
            {
                //衝突地点を最後のポイントにする
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1,hit.point);

                omenObject.transform.position = hit.point;
                omenObject.SetActive(true);

                break;
            }

            //ポイント追加
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1,nextPosition);
            previousPosition = nextPosition;
        }
    }

    private Vector3 CalculateTrajectoryPosition(Vector3 startPosition,Vector3 velocity,float time)
    {
        // 重力
        Vector3 gravity = Vector3.down * gravityPower;
        // 放物線
        return startPosition + velocity * time + 0.5f * gravity * time * time;
    }
}