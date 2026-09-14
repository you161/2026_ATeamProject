using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private PlayerMove playerMove = null;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isFrontJumping = false;
    private float currentJumpPower = 0f;
    private float currentTime = 0f;

    public bool IsJumping { get => isJumping; }
    public bool IsFrontJumping { get => isFrontJumping; }

    private void Start()
    {
        isGrounded = true;
        isJumping = false;
    }
    private void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
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
            if (isGrounded)
            {
                currentJumpPower = playerData.jumpPower;
                isGrounded = false;
                isJumping = true;
            }
            return;
        }

        if (!isFrontJumping)
        {
            isFrontJumping = true;
            currentTime = 0;
        }
    }

    private void Jumping()
    {
        if (isJumping)
        {
            Vector3 pos = rb.position;
            pos += currentJumpPower * Time.fixedDeltaTime * Vector3.up;
            currentJumpPower -= playerData.gravityPower * Time.fixedDeltaTime;
            rb.position = pos;
        }
    }

    private void FrontJumping()
    {
        if (isFrontJumping)
        {
            Vector3 pos = rb.position;
            pos += Time.fixedDeltaTime * playerMove.CurrentVelocity;
            rb.position = pos;
            //playerData.frontJumpPower

            currentTime += Time.fixedDeltaTime;
            if (currentTime >= playerData.frontJumpTime)
            {
                isFrontJumping = false;
                currentTime = 0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            isGrounded = true;
            isJumping = false;
            isFrontJumping = false;
            currentJumpPower = 0f;
        }
    }
}