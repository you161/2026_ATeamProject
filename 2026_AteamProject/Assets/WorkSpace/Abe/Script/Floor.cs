using Unity.Collections.Tests.CoreCLR.TestJobs;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private float Timer = 0.0f;
    private bool isFalling = false;
    private bool isReset = false;

    void Start()
    {
        StartPosition = transform.position;
        BaseMaterial = myRenderer.material;
    }

    void Update()
    {
        if (isFalling && !isReset)
        {
            Timer += Time.deltaTime;

            if (Timer > FallingTime)
            {
                Vector3 currentPosition = transform.position;
                currentPosition.y += FallingSpeed * Time.deltaTime;
                transform.position = currentPosition;
                Color color = myRenderer.material.color;
                color.a += vanishSpeed * Time.deltaTime;
                color.a = Mathf.Max(0.0f, color.a);
                myRenderer.material.color = color;

                if (myRenderer.material.color.a == 0.0f)
                {
                    isReset = true;
                    Timer = 0.0f;
                }
            }
        }
        else if (isReset && isLoop)
        {
            Timer += Time.deltaTime;

            if (myRenderer.material == BaseMaterial)
            {
                Color color = myRenderer.material.color;
                color.a = Timer % 0.2f < 0.1 ? 0.0f : 1.0f;
                myRenderer.material.color = color;

                if (Timer > FlashTime)
                {
                    myRenderer.material = BaseMaterial;
                    isFalling = false;
                    isReset = false;
                    Timer = 0.0f;
                }
            }
            else if (Timer > ResetTime)
            {
                transform.position = StartPosition;
                myRenderer.material = BaseMaterial;
                Timer = 0.0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFalling)
        {
            isFalling = true;
            transform.localScale = new Vector3(Resize, Resize, Resize);
            myRenderer.material = changeMaterial;
        }
    }

    private void OnPlayer()
    {
        
       
    }
}
