using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject controllerUI = null;
    [SerializeField] private GameObject keyboardUI = null;
    [SerializeField] private GameSceneManager gameSceneManager = null;
    [SerializeField] private SEManager seManager = null;

    private bool hasController = false;
    private bool isQuitting = false;
    private void Start()
    {
        hasController = false;
        isQuitting = false;

        hasController = Gamepad.all.Count > 0;

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
    }

    private void Update()
    {
        if (hasController && Gamepad.current.aButton.wasPressedThisFrame
            || Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            gameSceneManager.LoadTutorialScene();
            seManager.GameStartButtonSE();
        }

        if (!isQuitting)
        {
            if(hasController && Gamepad.current.bButton.wasPressedThisFrame
                || Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                isQuitting = true;
                Application.Quit();
            }
        }
    }
}