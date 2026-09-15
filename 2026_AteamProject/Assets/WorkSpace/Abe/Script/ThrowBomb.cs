using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowBomb : MonoBehaviour
{
    [SerializeField] private GameObject BombObject;

    void Start()
    {
        
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Instantiate(BombObject,
                new Vector3(transform.position.x, transform.position.y - 1, 0.0f),
                Quaternion.identity);
            
        }
    }
}
