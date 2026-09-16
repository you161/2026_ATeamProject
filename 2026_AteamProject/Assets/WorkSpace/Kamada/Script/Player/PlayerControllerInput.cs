using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerInput : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = null;
    private InputAction moveAction = null;
    private InputAction southButtonAction = null;
    private InputAction eastButtonAction = null;

    public Vector2 MoveInput
    {
        get
        {
            return moveAction.ReadValue<Vector2>();
        }
    }
    public bool SouthButtonPressed
    {
        get
        {
            return southButtonAction.WasPressedThisFrame();
        }
    }
    public bool EastButtonPressed
    {
        get
        {
            return eastButtonAction.WasPressedThisFrame();
        }
    }

    private void Awake()
    {
        moveAction = playerInput.actions["Move"];
        southButtonAction = playerInput.actions["PushSouthButton"];
        eastButtonAction = playerInput.actions["PushEastButton"];
    }
}