using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private Material changeMaterial;
    [SerializeField] private float Resize = 1.0f;
    [SerializeField] private float FallingTime = 0.0f;
    [SerializeField] private float FallingSpeed = 0.0f;
    [SerializeField] private float vanishSpeed = 0.0f;
    [SerializeField] private float ResetTime = 0.0f;
    [SerializeField] private float FlashTime = 0.0f;
    [SerializeField] private bool isLoop = false;

    private Vector3 StartPosition;
    private Material BaseMaterial;
    private float countTime = 0.0f;
    private bool isFalling = false;
    private bool isReset = false;

    void Start()
    {
        StartPosition = transform.position;
        BaseMaterial = myRenderer.material;
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

            if (countTime > FallingTime)
            {
                Vector3 currentPosition = transform.position;
                currentPosition.y += FallingSpeed * Time.deltaTime;
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

            if (transform.position == StartPosition)
            {
                Color color = myRenderer.material.color;
                color.a = countTime % 0.2f < 0.1 ? 0.0f : 1.0f;

                if (countTime > FlashTime)
                {
                    color.a = 1.0f;
                    isFalling = false;
                    isReset = false;
                    countTime = 0.0f;
                }

                myRenderer.material.color = color;
            }
            else if (countTime > ResetTime)
            {
                transform.position = StartPosition;
                myRenderer.material = BaseMaterial;
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
            transform.GetChild(0).gameObject.transform.localScale = new Vector3(Resize, Resize, Resize);
            myRenderer.material = changeMaterial;
        }
        if (collision.gameObject.CompareTag("Bomb"))
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
