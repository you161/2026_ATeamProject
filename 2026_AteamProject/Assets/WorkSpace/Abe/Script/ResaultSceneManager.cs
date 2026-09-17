using UnityEngine;
using UnityEngine.InputSystem;

public class ResaultSceneManager : MonoBehaviour
{
    [SerializeField] private GameSceneManager gameSceneManeger = null;

    public bool IsWait = false;

    void Update()
    {
        var gamepad = Gamepad.current;

        if (IsWait)
        {
            if ((gamepad != null && Gamepad.current.bButton.wasPressedThisFrame) || 
                Keyboard.current.shiftKey.wasPressedThisFrame)
            {
                gameSceneManeger.LoadMainScene();
            }
            if ((gamepad != null && Gamepad.current.aButton.wasPressedThisFrame) || 
                Keyboard.current.ctrlKey.wasPressedThisFrame)
            {
                gameSceneManeger.LoadTitleScene();
            }
        }
    }
}
