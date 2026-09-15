using UnityEngine;

public class BombController : MonoBehaviour
{
    //[SerializeField] private GameObject BombObject;
    [SerializeField] private float PulsationSize = 0.0f;
    [SerializeField] private float PulsationTime = 0.0f;
    [SerializeField] private float ThrowAxis = 0.0f;
    [SerializeField] private float ThrowDistance = 0.0f;
    [SerializeField] private float ThrowSpeed = 0.0f;
    [SerializeField] private float FallingSpeed = 1.0f;
    [SerializeField] private float ExplosionTime = 0.0f;
    [SerializeField] private float ExplosionSize = 0.0f;
    [SerializeField] private float Knockback = 0.0f;
    [SerializeField] private bool ImpactExplosion = true;

    private float countTime = 0.0f;
    private bool isLift = false;
    private bool isExplosion = false;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x += ThrowDistance * Time.deltaTime;
        if (ThrowAxis >= 0.0f)
        {
            currentPosition.y += FallingSpeed * -1.0f / ThrowDistance * ThrowAxis * ThrowAxis * Time.deltaTime;
        }
        else
        {
            currentPosition.y -= -1.0f / ThrowDistance * ThrowAxis * ThrowAxis * Time.deltaTime;
        }
        ThrowAxis += ThrowSpeed * Time.deltaTime;
        if (-0.5f < ThrowAxis && ThrowAxis < 0.5f) ThrowAxis = 0.5f;
        transform.position = currentPosition;

        if (transform.position.y < 0.0f) Destroy(gameObject);
    }
}
