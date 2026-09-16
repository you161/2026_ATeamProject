using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKnockback : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody = null;
    [SerializeField] private PlayerData playerData = null;
    private Vector3 knockbackDirection = Vector3.zero;
    private float currentTime = 0;
    private bool isKnockback = false;
    public bool IsKnockback { get => isKnockback; }
    private void Start()
    {
        if(playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody>();
        }
    }
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            PlayKnockback(new Vector3(0, 0, 0));
        }
    }
    private void FixedUpdate()
    {
        Knockback();
    }
    public void PlayKnockback(Vector3 targetPosition)
    {
        if (isKnockback)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, targetPosition);
        
        if(distance > playerData.KnockbackDistance)
        {
            return;
        }

        Vector3 direction = transform.position - targetPosition;
        direction.y = 0;
        direction.Normalize();
        knockbackDirection = direction;
        isKnockback = true;
    }
    private void Knockback()
    {
        if(!isKnockback)
        {
            return;
        }

        Vector3 playerVelocity = playerRigidbody.linearVelocity;
        playerVelocity += playerData.KnockbackPower * Time.fixedDeltaTime * knockbackDirection;
        playerRigidbody.linearVelocity = playerVelocity;

        currentTime += Time.fixedDeltaTime;
        if(currentTime >= playerData.KnockbackTime)
        {
            isKnockback = false;
            playerRigidbody.linearVelocity = Vector3.zero;
            currentTime = 0;
        }
    }
}