using UnityEngine;

public class BombCollider : MonoBehaviour
{
    private bool isFlag = false;
    public bool IsFlag { get => isFlag; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isFlag = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isFlag = true;
        }
    }
}
