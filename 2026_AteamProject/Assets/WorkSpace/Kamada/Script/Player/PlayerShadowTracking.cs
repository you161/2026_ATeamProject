using UnityEngine;

public class PlayerShadowTracking : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private GameObject shadowObject = null;
    [SerializeField] private float shadowOffset = 0;
    private void Update()
    {
        //影の位置を更新
        if (Physics.Raycast(rb.position, Vector3.down, out RaycastHit hit))
        {
            if (!shadowObject.activeSelf)
            {
                shadowObject.SetActive(true);
            }

            shadowObject.transform.position = hit.point + Vector3.up * shadowOffset;
        }
        else
        {
            if (shadowObject.activeSelf)
            {
                shadowObject.SetActive(false);
            }
        }
    }
}