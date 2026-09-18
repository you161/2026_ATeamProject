using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Image tutorialImage = null;
    [SerializeField] private Sprite[] tutorialSprites = null;
    [SerializeField] private GameSceneManager gameSceneManager = null;
    [SerializeField] private SEManager seManager = null;
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
    }

    private void Update()
    {
        if (hasController && Gamepad.current.aButton.wasPressedThisFrame 
            || Keyboard.current.dKey.wasPressedThisFrame)
        {
            NextPage();
        }

        if (hasController && Gamepad.current.bButton.wasPressedThisFrame
            || Keyboard.current.aKey.wasPressedThisFrame)
        {
            PreviousPage();
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
                seManager.NextPageSE();
            }
            return;
        }

        if (pageNum >= 0 && pageNum < tutorialSprites.Length)
        {
            currentPage = pageNum;
            tutorialImage.sprite = tutorialSprites[currentPage];
            seManager.NextPageSE();
        }
    }
}