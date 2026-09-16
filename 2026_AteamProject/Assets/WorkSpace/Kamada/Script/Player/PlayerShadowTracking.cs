using UnityEngine;

public class PlayerShadowTracking : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private GameObject shadowObject = null;
    private bool isFalling = false;
    public bool IsFalling { get => isFalling; }

    private void Update()
    {
        //影の位置を更新
        if (Physics.Raycast(rb.position, Vector3.down, out RaycastHit hit))
        {
            shadowObject.transform.position = hit.point;

            if (isFalling)
            {
                isFalling = false;
                rb.useGravity = false;
            }
        }
        else
        {
            if (!isFalling)
            {
                isFalling = true;
                rb.useGravity = true;
            }

            shadowObject.transform.position = rb.position + Vector3.down * 5.0f;
        }
    }
}