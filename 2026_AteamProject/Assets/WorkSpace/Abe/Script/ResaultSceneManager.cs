using UnityEngine;
using UnityEngine.InputSystem;

public class ResaultSceneManager : MonoBehaviour
{
    [SerializeField] private GameSceneManager gameSceneManeger = null;
    [SerializeField] private GameObject controllerUI = null;
    [SerializeField] private GameObject keyboardUI = null;

    public bool IsWait = false;
    private bool hasController = false;
    private bool isActive = false;

    private void Start()
    {
        hasController = false;
        isActive = false;
        hasController = Gamepad.all.Count > 0;

        controllerUI.SetActive(false);
        keyboardUI.SetActive(false);
    }
    private void Update()
    {
        if (IsWait)
        {
            if (!isActive)
            {
                if (hasController)
                {
                    controllerUI.SetActive(true);
                    keyboardUI.SetActive(false);
                }
                else
                {
                    controllerUI.SetActive(false);
                    keyboardUI.SetActive(true);
                }

                isActive = true;
            }

            if ((hasController && Gamepad.current.aButton.wasPressedThisFrame) || 
                Keyboard.current.shiftKey.wasPressedThisFrame)
            {
                gameSceneManeger.LoadMainScene();
            }

            if ((hasController && Gamepad.current.bButton.wasPressedThisFrame) || 
                Keyboard.current.ctrlKey.wasPressedThisFrame)
            {
                gameSceneManeger.LoadTitleScene();
            }
        }
    }
}
