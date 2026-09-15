using UnityEngine;

public class BombController : MonoBehaviour
{
    //[SerializeField] private GameObject BombObject;
    [SerializeField] private LineRenderer myLineRenderer;
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

    public bool isLift = false;
    public bool isThrow = false;

    private float countTime = 0.0f;
    private bool isExplosion = false;

    void Start()
    {
        
    }

    void Update()
    {
        countTime += Time.deltaTime;
        if (countTime > ExplosionTime)
        {
            isExplosion = true;
        }

        if (isThrow)
        {
            //myLineRenderer.positionCount = 0;
            Vector3 currentPosition = transform.position;
            currentPosition.x += ThrowDistance * Time.deltaTime;
            if (ThrowAxis >= 0.0f)
            {
                currentPosition.y += FallingSpeed * -1.0f / ThrowDistance * ThrowSpeed * ThrowAxis * ThrowAxis * Time.deltaTime;
            }
            else
            {
                currentPosition.y -= -1.0f / ThrowDistance * ThrowSpeed * ThrowAxis * ThrowAxis * Time.deltaTime;
            }
            ThrowAxis += ThrowSpeed * Time.deltaTime;
            if (-1.0f < ThrowAxis && ThrowAxis < 1.0f) ThrowAxis = 1.0f;
            transform.position = currentPosition;
        }

        if (isLift)
        {
            int segmentCount = 50;
            int actualPoints = 0;
            float timeStep = 0.005f;
            myLineRenderer.positionCount = segmentCount;
            Vector3 point = transform.position;
            float startAxis = ThrowAxis;

            for (int i = 0; i < segmentCount; ++i)
            {
                float t = i * timeStep;
                point.x += ThrowDistance * t;
                if (startAxis >= 0.0f)
                {
                    point.y += FallingSpeed * -1.0f / ThrowDistance * ThrowSpeed * startAxis * startAxis * t;
                }
                else
                {
                    point.y -= -1.0f / ThrowDistance * ThrowSpeed * startAxis * startAxis * t;
                }
                startAxis += ThrowSpeed * t;
                if (-1.0f < startAxis && startAxis < 1.0f) startAxis = 1.0f;

                if (point.y <= 0.0f)
                {
                    myLineRenderer.SetPosition(i, point);
                    actualPoints = i + 1;
                    break;
                }
                myLineRenderer.SetPosition(i, point);
            }
            myLineRenderer.positionCount = actualPoints;
        }

        if (isExplosion)
        {

            Destroy(gameObject);
        }
    }
}
