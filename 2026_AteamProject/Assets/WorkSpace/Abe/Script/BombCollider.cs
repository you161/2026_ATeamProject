using UnityEngine;

public class BombCollider : MonoBehaviour
{
    private Bomb bombScript = null;

    private bool isFlag = false;
    public bool IsFlag { get => isFlag; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && bombScript.IsThrow)
        {
            isFlag = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isFlag = true;
        }
    }

    public void Getbomb(Bomb script)
    {
        bombScript = script;
    }
}
