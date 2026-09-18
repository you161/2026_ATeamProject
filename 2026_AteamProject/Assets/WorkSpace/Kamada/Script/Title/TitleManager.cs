using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject controllerUI = null;
    [SerializeField] private GameObject keyboardUI = null;
    [SerializeField] private GameSceneManager gameSceneManager = null;

    private bool hasController = false;
    private bool isQuitting = false;
    private void Start()
    {
        hasController = false;
        isQuitting = false;

        hasController = Gamepad.all.Count > 0;

        Debug.Log(hasController);

        //if (hasController)
        //{
        //    controllerUI.SetActive(true);
        //    keyboardUI.SetActive(false);
        //}
        //else
        //{
        //    controllerUI.SetActive(true);
        //    keyboardUI.SetActive(false);
        //}
    }

    private void Update()
    {
        //if (hasController)
        //{
        //    if (Gamepad.current.aButton.wasPressedThisFrame)
        //    {
        //        gameSceneManager.LoadMainScene();
        //    }

        //    if (!isQuitting && Gamepad.current.bButton.wasPressedThisFrame)
        //    {
        //        isQuitting = true;
        //        Application.Quit();
        //    }
        //}
        //else
        //{
        //    if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        //    {
        //        gameSceneManager.LoadMainScene();
        //    }

        //    if (!isQuitting && Keyboard.current.escapeKey.wasPressedThisFrame)
        //    {
        //        isQuitting = true;
        //        Application.Quit();
        //    }
        //}
    }
}