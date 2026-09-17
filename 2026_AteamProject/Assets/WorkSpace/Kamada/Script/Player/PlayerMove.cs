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
        isPressed = false;
        isMove = false;

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
        if (countDown.IsPlayCount && !countDown.IsGo)
        {
            currentVelocity = Vector3.zero;
            moveDirection = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            return;
        }

        if (playerKnockback.IsKnockback)
        {
            return;
        }

        MoveInput();
    }

    private void FixedUpdate()
    {
        if (countDown.IsPlayCount &&!countDown.IsGo)
        {
            return;
        }

        if (playerKnockback.IsKnockback)
        {
            return;
        }

        Move();
        Rotate();
    }

    private void MoveInput()
    {
        //毎フレームリセット
        moveDirection = Vector3.zero;
        isPressed = false;

        Vector2 input = playerControllerInput.MoveInput;

        //入力があるか
        if (input.sqrMagnitude > 0.01f)
        {
            isPressed = true;
            moveDirection = new Vector3(input.x,0,input.y);
            moveDirection.Normalize();
        }
    }

    private void Move()
    {
        float currentAcceleration = playerData.Acceleration;

        if (isPressed)
        {
            Vector3 targetVelocity = moveDirection * playerData.MaxMoveSpeed;

            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                targetVelocity,
                currentAcceleration * Time.fixedDeltaTime
            );

            isMove = true;
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                Vector3.zero,
                playerData.Deceleration * Time.fixedDeltaTime
            );

            isMove = false;
        }

        if (playerJump.IsFrontJumping)
        {
            Vector3 velocity = playerJump.FrontJumpVelocity;
            //前ジャンプ中は操作量を10%にする
            Vector3 controlVelocity = currentVelocity * 0.1f;

            velocity += new Vector3(controlVelocity.x,0f,controlVelocity.z);

            rb.linearVelocity = new Vector3(velocity.x,rb.linearVelocity.y,velocity.z);

            return;
        }

        rb.linearVelocity = new Vector3(
            currentVelocity.x,
            rb.linearVelocity.y,
            currentVelocity.z
        );
    }

    private void Rotate()
    {
        float rotationSpeed = playerData.RotationSpeed;

        //前ジャンプ中は回転速度を1/10にする
        if (playerJump.IsFrontJumping)
        {
            rotationSpeed *= 0.1f;
        }

        if (!isPressed)
        {
            return;
        }

        if (moveDirection.sqrMagnitude <= 0.01f)
        {
            return;
        }

        //移動方向に応じた目標回転
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        //現在の回転と目標の回転の角度
        float angle = Quaternion.Angle(rb.rotation, targetRotation);

        if (angle >= 150.0f)
        {
            //大きく方向転換する場合は回転速度を倍にする
            rotationSpeed *= 2.0f;
        }

        //目標方向へ回転
        rb.rotation = Quaternion.RotateTowards(
            rb.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }
}