using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Image tutorialImage = null;
    [SerializeField] private Sprite[] tutorialSprites = null;
    [SerializeField] private GameObject controllerUI = null;
    [SerializeField] private GameObject keyboardUI = null;
    [SerializeField] private GameSceneManager gameSceneManager = null;
    private int currentPage = 0;
    private bool isSceneChange = false;
    private bool hasController = false;
    private void Start()
    {
        currentPage = 0;
        tutorialImage.sprite = tutorialSprites[currentPage];
        isSceneChange = false;
        hasController = false;

        hasController = Gamepad.all.Count > 0;

        Debug.Log(hasController);

        if (hasController)
        {
            controllerUI.SetActive(true);
            keyboardUI.SetActive(false);
        }
        else
        {
            controllerUI.SetActive(true);
            keyboardUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (hasController)
        {
            if (Gamepad.current.aButton.wasPressedThisFrame)
            {
                NextPage();
            }

            if (Gamepad.current.bButton.wasPressedThisFrame)
            {
                PreviousPage();
            }
        }
        else
        {
            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                NextPage();
            }

            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                PreviousPage();
            }
        }
    }
    private void NextPage()
    {
        ChangePage(currentPage + 1);
    }

    private void PreviousPage()
    {
        ChangePage(currentPage - 1);
    }

    private void ChangePage(int pageNum)
    {
        if (pageNum >= tutorialSprites.Length)
        {
            if (!isSceneChange)
            {
                isSceneChange = true;
                gameSceneManager.LoadMainScene();
            }
            return;
        }

        if (pageNum >= 0 && pageNum < tutorialSprites.Length)
        {
            currentPage = pageNum;
            tutorialImage.sprite = tutorialSprites[currentPage];
        }
    }
}