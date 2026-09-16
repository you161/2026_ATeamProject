using UnityEngine;

public class BombCollider : MonoBehaviour
{
    private bool isFlag = false;
    public bool IsFlag { get => isFlag; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isFlag = true;
        }
    }
}
