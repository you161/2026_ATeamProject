using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("マテリアル")]
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private Material changeMaterial;

    [Header("落下設定")]
    [SerializeField] private float fallingSize = 1.0f;
    [SerializeField] private float fallingTime = 0.0f;
    [SerializeField] private float fallingSpeed = 0.0f;

    [Header("リセット設定")]
    [SerializeField] private float vanishSpeed = 0.0f;
    [SerializeField] private float resetTime = 0.0f;
    [SerializeField] private float flashTime = 0.0f;
    [SerializeField] private bool isLoop = false;

    private Vector3 startPosition;
    private Material baseMaterial;
    private float countTime = 0.0f;
    private bool isFalling = false;
    private bool isReset = false;

    void Start()
    {
        startPosition = transform.position;
        baseMaterial = myRenderer.material;
    }

    void Update()
    {
        if (isFalling)
        {
            FallingFloor();
        }
        ResetFloor();

    }

    private void FallingFloor()
    {
        if (!isReset)
        {
            countTime += Time.deltaTime;

            if (countTime > fallingTime)
            {
                Vector3 currentPosition = transform.position;
                currentPosition.y += fallingSpeed * Time.deltaTime;
                transform.position = currentPosition;
                Color color = myRenderer.material.color;
                color.a += vanishSpeed * Time.deltaTime;
                color.a = Mathf.Max(0.0f, color.a);
                myRenderer.material.color = color;

                if (currentPosition.y < -10.0f)
                {
                    transform.GetChild(0).gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    isReset = true;
                    countTime = 0.0f;
                }
            }
        }

    }

    private void ResetFloor()
    {
        if (isReset && isLoop)
        {
            countTime += Time.deltaTime;

            if (transform.position == startPosition)
            {
                Color color = myRenderer.material.color;
                color.a = countTime % 0.2f < 0.1 ? 0.0f : 1.0f;

                if (countTime > flashTime)
                {
                    color.a = 1.0f;
                    isFalling = false;
                    isReset = false;
                    countTime = 0.0f;
                }

                myRenderer.material.color = color;
            }
            else if (countTime > resetTime)
            {
                transform.position = startPosition;
                myRenderer.material = baseMaterial;
                countTime = 0.0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isReset)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            isFalling = true;
            transform.GetChild(0).gameObject.transform.localScale = new Vector3(fallingSize, fallingSize, fallingSize);
            myRenderer.material = changeMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bomb"))
        {
            Color color = myRenderer.material.color;
            color.a = 0.0f;
            myRenderer.material.color = color;
            transform.position = new Vector3(transform.position.x, -10.0f, transform.position.z);
            countTime = 0.0f;
            isReset = true;
        }
    }
}
