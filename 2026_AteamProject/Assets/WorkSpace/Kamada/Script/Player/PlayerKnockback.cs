using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody = null;
    [SerializeField] private PlayerData playerData = null;

    private Vector3 knockbackDirection = Vector3.zero;
    private float currentTime = 0.0f;
    private bool isKnockback = false;

    public bool IsKnockback { get => isKnockback; }

    private void Start()
    {
        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody>();
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

        float distance = Vector3.Distance(transform.position,targetPosition);

        if (distance > playerData.KnockbackDistance)
        {
            return;
        }

        //ノックバック方向
        Vector3 direction = transform.position - targetPosition;

        //Y方向は無視
        direction.y = 0.0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            direction.Normalize();
        }
        else
        {
            return;
        }

        knockbackDirection = direction;

        currentTime = 0.0f;
        isKnockback = true;
    }

    private void Knockback()
    {
        if (!isKnockback)
        {
            return;
        }

        //ノックバック速度を加える
        Vector3 knockbackVelocity = playerData.KnockbackPower * knockbackDirection;
        Vector3 velocity = playerRigidbody.linearVelocity;

        velocity += knockbackVelocity * Time.fixedDeltaTime;

        playerRigidbody.linearVelocity = velocity;

        currentTime += Time.fixedDeltaTime;

        if (currentTime >= playerData.KnockbackTime)
        {
            isKnockback = false;
            currentTime = 0.0f;

            //ノックバックによる速度を止める
            playerRigidbody.linearVelocity = Vector3.zero;
        }
    }
}