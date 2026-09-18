using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private PlayerJump playerJump = null;
    [SerializeField] private PlayerControllerInput playerControllerInput = null;
    [SerializeField] private PlayerKnockback playerKnockback = null;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;

    private bool isPressed = false;
    private bool isMove = false;

    public bool IsMove { get => isMove; }

    private CountDown countDown;

    private void Start()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        moveDirection = Vector3.zero;
        currentVelocity = Vector3.zero;

        isPressed = false;
        isMove = false;

        //ゲームオブジェクトからカウントダウン処理を探す
        GameObject countDownObj = GameObject.Find("CountDownManager");

        if (countDownObj != null)
        {
            countDown = countDownObj.GetComponent<CountDown>();
        }

        if (countDown == null)
        {
            Debug.Log("countDown is null");
        }
    }

    private void Update()
    {
        if (countDown != null && countDown.IsPlayCount && !countDown.IsGo)
        {
            currentVelocity = Vector3.zero;
            moveDirection = Vector3.zero;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            return;
        }

        MoveInput();
    }

    private void FixedUpdate()
    {
        if (countDown != null && countDown.IsPlayCount && !countDown.IsGo)
        {
            return;
        }

        Move();
        Rotate();
    }

    private void MoveInput()
    {
        moveDirection = Vector3.zero;
        isPressed = false;

        Vector2 input = playerControllerInput.MoveInput;

        //入力があるか
        if (input.sqrMagnitude > 0.01f)
        {
            isPressed = true;
            moveDirection = new Vector3(input.x,0.0f,input.y);
            moveDirection.Normalize();
        }
    }

    private void Move()
    {
        float currentAcceleration = playerData.Acceleration;

        if (playerKnockback.IsKnockback)
        {
            MoveKnockback();
            return;
        }

        if (isPressed)
        {
            Vector3 targetVelocity = moveDirection * playerData.MaxMoveSpeed;

            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                targetVelocity,
                currentAcceleration *
                Time.fixedDeltaTime
            );

            isMove = true;
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                Vector3.zero,
                playerData.Deceleration *
                Time.fixedDeltaTime
            );

            isMove = false;
        }

        if (playerJump.IsFrontJumping)
        {
            Vector3 velocity = playerJump.FrontJumpVelocity;

            //前ジャンプ中は操作量変更
            Vector3 controlVelocity = currentVelocity * playerData.frontJumpingMoveRate;
            velocity += new Vector3(controlVelocity.x,0.0f,controlVelocity.z);
            rb.linearVelocity = new Vector3(velocity.x,rb.linearVelocity.y,velocity.z);

            return;
        }

        rb.linearVelocity = new Vector3(
            currentVelocity.x,
            rb.linearVelocity.y,
            currentVelocity.z
        );
    }

    private void MoveKnockback()
    {
        Vector3 targetVelocity = Vector3.zero;

        if (isPressed)
        {
            targetVelocity = playerData.knockbackMoveRate * playerData.MaxMoveSpeed * moveDirection;
        }

        currentVelocity = Vector3.MoveTowards(
            currentVelocity,
            targetVelocity,
            playerData.Acceleration *
            Time.fixedDeltaTime
        );

        Vector3 velocity = rb.linearVelocity;

        velocity.x = currentVelocity.x;
        velocity.z = currentVelocity.z;

        rb.linearVelocity = velocity;

        isMove = isPressed;
    }

    private void Rotate()
    {
        float rotationSpeed = playerData.RotationSpeed;

        //前ジャンプ中回転速度を変更
        if (playerJump.IsFrontJumping)
        {
            rotationSpeed *= playerData.frontJumpingMoveRate;
        }

        //ノックバック中はさらに回転速度を変更
        if (playerKnockback.IsKnockback)
        {
            rotationSpeed *= playerData.knockbackMoveRate;
        }

        if (!isPressed)
        {
            return;
        }

        if (moveDirection.sqrMagnitude <= 0.01f)
        {
            return;
        }

        //移動方向に向ける
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        //現在の回転と目標回転の角度
        float angle = Quaternion.Angle(rb.rotation,targetRotation);

        //大きく方向転換する場合回転速度を倍に
        if (angle >= 150.0f)
        {
            rotationSpeed *= 2.0f;
        }

        //目標方向へ回転
        rb.rotation = Quaternion.RotateTowards(
            rb.rotation,
            targetRotation,
            rotationSpeed *
            Time.fixedDeltaTime
        );
    }
}