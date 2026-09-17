using UnityEngine;
using UnityEngine.InputSystem;

public class ResaultSceneManager : MonoBehaviour
{
    [SerializeField] private GameSceneManager gameSceneManeger = null;

    public bool IsWait = false;

    void Update()
    {
        if (IsWait)
        {
            if (Gamepad.current.bButton.wasPressedThisFrame)
            {
                gameSceneManeger.LoadMainScene();
            }
            if (Gamepad.current.aButton.wasPressedThisFrame)
            {
                gameSceneManeger.LoadTitleScene();
            }
        }
    }
}
