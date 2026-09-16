using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private PlayerJump playerJump = null;
    [SerializeField] private PlayerControllerInput playerControllerInput = null;
    [SerializeField] private GameObject shadowObject = null;
    [SerializeField] private PlayerKnockback playerKnockback = null;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;
    private bool isPressed = false;

    private void Start()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        moveDirection = Vector3.zero;
        isPressed = false;
    }

    private void Update()
    {
        if (playerKnockback.IsKnockback)
        {
            return;
        }

        MoveInput();
    }

    private void FixedUpdate()
    {
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
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                Vector3.zero,
                playerData.Deceleration * Time.fixedDeltaTime
            );
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

        if (!isPressed && !playerJump.IsFrontJumping)
        {
            Vector3 velocity = rb.linearVelocity;

            velocity.x = 0f;
            velocity.z = 0f;

            rb.linearVelocity = velocity;

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