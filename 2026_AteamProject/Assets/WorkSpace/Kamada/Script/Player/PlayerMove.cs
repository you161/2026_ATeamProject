using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private PlayerJump playerJump = null;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;
    private bool isPressed = false;
    public Vector3 CurrentVelocity { get => currentVelocity; }

    private void Start()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        moveDirection = Vector3.zero;
    }
    private void Update()
    {
        MoveInput();
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void MoveInput()
    {
        isPressed = false;

        if (Keyboard.current.wKey.isPressed)
        {
            moveDirection += Vector3.forward;
            isPressed = true;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            moveDirection -= Vector3.forward;
            isPressed = true;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            moveDirection -= Vector3.right;
            isPressed = true;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            moveDirection += Vector3.right;
            isPressed = true;
        }

        moveDirection.Normalize();
    }
    private void Move()
    {
        float currentRotationSpeed = playerData.rotationSpeed;
        float currentAcceleration = playerData.acceleration;

        if (playerJump.IsFrontJumping)
        {
            currentRotationSpeed *= 0.5f;
            currentAcceleration *= 0.5f;
        }

        if (isPressed)
        {
            // 移動入力がある場合、現在の速度を目標速度に向かって加速させる
            Vector3 targetDirection = moveDirection * playerData.maxMoveSpeed;
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                targetDirection,
                currentAcceleration * Time.fixedDeltaTime
            );

            // 移動方向とは別に、プレイヤーの向きを徐々に変更
            if (currentVelocity.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);
                rb.rotation = Quaternion.RotateTowards(
                    rb.rotation,
                    targetRotation,
                    currentRotationSpeed * Time.fixedDeltaTime
                );
            }
        }
        else
        {
            //プレイヤーが移動入力をしていない場合、速度を減速させる
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                Vector3.zero,
                playerData.deceleration * Time.fixedDeltaTime
            );
        }

        //LinerVelocityのy軸の値を保持しつつ、x軸とz軸の値を更新
        rb.linearVelocity = new Vector3(
            currentVelocity.x,
            rb.linearVelocity.y,
            currentVelocity.z
        );
    }
}