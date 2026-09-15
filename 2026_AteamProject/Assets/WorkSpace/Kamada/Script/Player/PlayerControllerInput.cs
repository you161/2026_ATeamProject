using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerInput : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction pushAction;

    public Vector2 MoveInput
    {
        get
        {
            return moveAction.ReadValue<Vector2>();
        }
    }
    public bool PushPressed
    {
        get
        {
            return pushAction.WasPressedThisFrame();
        }
    }

    private void Awake()
    {
        moveAction = playerInput.actions["Move"];
        pushAction = playerInput.actions["Push"];
    }
}