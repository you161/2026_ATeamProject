using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private RaundManager raundManager = null;

    [Header("コライダー")]
    [SerializeField] private MeshCollider myCollider = null;

    [Header("マテリアル")]
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private Material changeMaterial;

    [Header("落下設定")]
    [SerializeField] private float fallingSize = 1.0f;
    [SerializeField] private float fallingTime = 0.0f;
    [SerializeField] private float fallingSpeed = 0.0f;
    [SerializeField] private float maxGround = -15.0f;

    [Header("リセット設定")]
    [SerializeField] private float vanishSpeed = 0.0f;
    [SerializeField] private float resetTime = 0.0f;
    [SerializeField] private float flashTime = 0.0f;
    [SerializeField] private bool isLoop = false;

    private Vector3 startPosition;
    private Material baseMaterial;
    private float fallingTimer = 0.0f;
    private float resetTimer = 0.0f;
    private bool isFalling = false;
    private bool isReset = false;

    void Start()
    {
        startPosition = transform.position;
        baseMaterial = myRenderer.material;
    }

    void Update()
    {
        // ステージの落下
        if (isFalling)
        {
            FallingFloor();
        }

        // ステージのリセット
        if (isReset && isLoop)
        {
            ResetFloor();
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            isFalling = false;
            isReset = false;
            fallingTimer = 0.0f;
            resetTimer = 0.0f;
            transform.GetChild(0).gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            transform.position = startPosition;
            myCollider.isTrigger = false;
            myRenderer.material = baseMaterial;
            Color color = myRenderer.material.color;
            color.a = 1.0f;
            myRenderer.material.color = color;
        }
    }

    private void FallingFloor()
    {
        fallingTimer += Time.deltaTime;

        if (fallingTimer > fallingTime)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.y += fallingSpeed * Time.deltaTime;
            transform.position = currentPosition;
            Color color = myRenderer.material.color;
            color.a += vanishSpeed * Time.deltaTime;
            color.a = Mathf.Max(0.0f, color.a);
            myRenderer.material.color = color;

            if (!isReset) isReset = true;

            if (currentPosition.y < maxGround)
            {
                myCollider.isTrigger = true;
                fallingTimer = 0.0f;
                isFalling = false;
            }
        }
    }

    private void ResetFloor()
    {
        resetTimer += Time.deltaTime;

        if (transform.position == startPosition)
        {
            Color color = myRenderer.material.color;
            color.a = resetTimer % 0.2f < 0.1 ? 0.0f : 1.0f;

            if (resetTimer > flashTime)
            {
                myCollider.isTrigger = false;
                color.a = 1.0f;
                isReset = false;
                resetTimer = 0.0f;
            }

            myRenderer.material.color = color;
        }
        else if (resetTimer > resetTime)
        {
            transform.GetChild(0).gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            transform.position = startPosition;
            myRenderer.material = baseMaterial;
            myCollider.isTrigger = true;
            isFalling = false;
            resetTimer = 0.0f;
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
        if (isReset)
        {
            return;
        }

        if (other.gameObject.CompareTag("Bomb"))
        {
            Color color = myRenderer.material.color;
            color.a = 0.0f;
            myRenderer.material.color = color;
            transform.position = new Vector3(transform.position.x, -10.0f, transform.position.z);
            myCollider.isTrigger = true;
            isReset = true;
        }
    }
}
