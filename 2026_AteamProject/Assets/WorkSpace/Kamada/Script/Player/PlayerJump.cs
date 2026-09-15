using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private Transform player = null;
    [SerializeField] private PlayerControllerInput playerControllerInput = null;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isFrontJumping = false;

    //通常ジャンプの上方向速度
    private float currentJumpPower = 0f;

    //前ジャンプの上方向速度
    private float currentFrontJumpPower = 0f;

    private float currentTime = 0f;

    public bool IsFrontJumping { get => isFrontJumping; }

    private void Start()
    {
        isGrounded = true;
        isJumping = false;
        isFrontJumping = false;

        rb.useGravity = false;
    }

    private void Update()
    {
        if (playerControllerInput.PushPressed)
        {
            OnJump();
        }
    }

    private void FixedUpdate()
    {
        Jumping();
        FrontJumping();
    }

    private void OnJump()
    {
        if (!isJumping)
        {
            if (!isGrounded)
            {
                return;
            }

            isGrounded = false;
            isJumping = true;

            currentJumpPower = playerData.jumpPower;

            return;
        }

        if (!isFrontJumping)
        {
            StartFrontJump();
        }
    }

    private void StartFrontJump()
    {
        isFrontJumping = true;
        currentTime = 0f;

        //前ジャンプ開始時の上方向速度
        currentFrontJumpPower = playerData.frontJumpUpPower;

        //キャラクターを前傾
        Vector3 playerAngle = player.localEulerAngles;
        playerAngle.x = 80f;
        player.localEulerAngles = playerAngle;
    }

    private void Jumping()
    {
        //ジャンプ中もしくは前ジャンプ中なら処理しない
        if (!isJumping || isFrontJumping)
        {
            return;
        }

        //上方向へ移動
        Vector3 pos = rb.position;
        pos += currentJumpPower * Time.fixedDeltaTime * Vector3.up;
        rb.position = pos;

        //重力
        currentJumpPower -= playerData.gravityPower * Time.fixedDeltaTime;
    }

    private void FrontJumping()
    {
        if (!isFrontJumping)
        {
            return;
        }

        Vector3 forward = rb.transform.forward;

        //前傾による上下方向の影響を除去
        forward.y = 0f;
        forward.Normalize();

        Vector3 pos = rb.position;

        pos += playerData.frontJumpPower * Time.fixedDeltaTime * forward;
        pos += currentFrontJumpPower * Time.fixedDeltaTime * Vector3.up;

        rb.position = pos;

        currentFrontJumpPower -= playerData.gravityPower * Time.fixedDeltaTime;

        currentTime += Time.fixedDeltaTime;

        if (currentTime >= playerData.frontJumpTime)
        {
            isFrontJumping = false;
            currentTime = 0f;

            //前傾を解除
            Vector3 playerAngle = player.localEulerAngles;
            playerAngle.x = 0f;
            player.localEulerAngles = playerAngle;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Finish"))
        {
            return;
        }

        isGrounded = true;
        isJumping = false;
        isFrontJumping = false;

        currentJumpPower = 0f;
        currentFrontJumpPower = 0f;
        currentTime = 0f;

        //前傾を解除
        Vector3 playerAngle = player.localEulerAngles;
        playerAngle.x = 0f;
        player.localEulerAngles = playerAngle;
    }
}