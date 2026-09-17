using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerInput : MonoBehaviour
{
    public enum InputType
    {
        GamePad,
        WASD,
        Arrow
    }

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

    public void SetInputType(InputType inputType)
    {
        if (playerInput == null)
        {
            Debug.LogError("PlayerInputがnullです");
            return;
        }

        switch (inputType)
        {
            case InputType.GamePad:
                moveAction = playerInput.actions["MoveGamePad"];
                southButtonAction = playerInput.actions["JumpGamePad"];
                eastButtonAction = playerInput.actions["BombGamePad"];
                break;

            case InputType.WASD:
                moveAction = playerInput.actions["MoveWASD"];
                southButtonAction = playerInput.actions["JumpKeyboard_0"];
                eastButtonAction = playerInput.actions["BombKeyboard_0"];
                break;

            case InputType.Arrow:
                moveAction = playerInput.actions["MoveArrow"];
                southButtonAction = playerInput.actions["JumpKeyboard_1"];
                eastButtonAction = playerInput.actions["BombKeyboard_1"];
                break;
        }
    }
}