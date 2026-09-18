using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private Transform player = null;
    [SerializeField] private PlayerControllerInput playerControllerInput = null;
    [SerializeField] private Bomb bomb = null;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isFrontJumping = false;

    // 前ジャンプ開始からの時間
    private float currentTime = 0f;
    private Vector3 frontJumpVelocity = Vector3.zero;

    public bool IsFrontJumping { get => isFrontJumping; }
    public bool IsJumping { get => isJumping; }

    public Vector3 FrontJumpVelocity { get => frontJumpVelocity; }

    private CountDown countDown;

    private void Start()
    {
        isGrounded = true;
        isJumping = false;
        isFrontJumping = false;

        rb.useGravity = false;

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
        //カウントダウン中
        if (countDown.IsPlayCount && !countDown.IsGo)
        {
            rb.useGravity = false;
            ResetJump();

            return;
        }

        //GO後
        if (!rb.useGravity)
        {
            rb.useGravity = true;
        }

        if (playerControllerInput.SouthButtonPressed)
        {
            OnJump();
        }
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        FrontJumping();
    }

    private void OnJump()
    {
        //通常ジャンプ
        if (!isJumping)
        {
            if (!isGrounded)
            {
                return;
            }

            isGrounded = false;
            isJumping = true;
            rb.useGravity = false;

            //ジャンプ開始時の速度を設定
            Vector3 velocity = rb.linearVelocity;
            velocity.y = playerData.JumpPower;
            rb.linearVelocity = velocity;

            return;
        }

        //前ジャンプ
        if (!isFrontJumping && !bomb.IsReady)
        {
            StartFrontJump();
        }
        else if (!isFrontJumping && bomb.IsReady && bomb.IsThrow)
        {
            StartFrontJump();
        }
    }

    private void ApplyGravity()
    {
        if (!isJumping)
        {
            return;
        }

        Vector3 velocity = rb.linearVelocity;
        velocity.y -= playerData.GravityPower * Time.fixedDeltaTime;
        rb.linearVelocity = velocity;
    }

    private void StartFrontJump()
    {
        isFrontJumping = true;
        currentTime = 0f;

        //前傾
        Vector3 playerAngle = player.localEulerAngles;
        playerAngle.x = 80f;
        player.localEulerAngles = playerAngle;

        //前方向
        Vector3 forward = rb.transform.forward;

        //前傾による上下方向の影響を除去
        forward.y = 0f;
        forward.Normalize();

        //前方向 + 上方向の速度を設定
        Vector3 velocity = rb.linearVelocity;

        velocity.x = forward.x * playerData.FrontJumpPower;
        velocity.z = forward.z * playerData.FrontJumpPower;
        velocity.y = playerData.FrontJumpUpPower;

        rb.linearVelocity = velocity;

        frontJumpVelocity = forward * playerData.FrontJumpPower;
    }

    private void FrontJumping()
    {
        if (!isFrontJumping)
        {
            return;
        }

        currentTime += Time.fixedDeltaTime;

        if (currentTime >= playerData.FrontJumpTime)
        {
            ResetFrontJump();
        }
    }

    private void ResetFrontJump()
    {
        isFrontJumping = false;
        currentTime = 0f;

        //前方向の速度を止める
        Vector3 velocity = rb.linearVelocity;
        velocity.x = 0f;
        velocity.z = 0f;
        rb.linearVelocity = velocity;

        ResetPlayerRotation();
    }

    private void ResetJump()
    {
        isGrounded = true;
        isJumping = false;
        isFrontJumping = false;

        currentTime = 0f;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        ResetPlayerRotation();
    }

    private void ResetPlayerRotation()
    {
        Vector3 playerAngle = player.localEulerAngles;
        playerAngle.x = 0f;
        player.localEulerAngles = playerAngle;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
        {
            return;
        }

        ResetJump();
    }
}