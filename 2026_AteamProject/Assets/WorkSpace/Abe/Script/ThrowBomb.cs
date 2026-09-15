using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowBomb : MonoBehaviour
{
    [SerializeField] private GameObject BombObject;

    private GameObject currentBonb;

    void Start()
    {
        
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (currentBonb == null)
            {
                currentBonb = Instantiate(BombObject,
                    new Vector3(transform.position.x, transform.position.y - 1, 0.0f),
                    Quaternion.identity);
                currentBonb.GetComponent<BombController>().isLift = true;
            }
            else if (currentBonb != null)
            {
                currentBonb.GetComponent<BombController>().isLift = false;
                currentBonb.GetComponent<BombController>().isThrow = true;
            }
        }
    }
}
