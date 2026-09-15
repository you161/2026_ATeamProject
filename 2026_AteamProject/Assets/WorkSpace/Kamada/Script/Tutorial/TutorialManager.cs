using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Image tutorialImage = null;
    [SerializeField] private Sprite[] tutorialSprites = null;
    private int currentPage = 0;
    private void Start()
    {
        currentPage = 0;
        tutorialImage.sprite = tutorialSprites[currentPage];
    }

    private void Update()
    {
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            NextPage();

        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
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
        if (pageNum >= 0 && pageNum < tutorialSprites.Length)
        {
            currentPage = pageNum;
            tutorialImage.sprite = tutorialSprites[currentPage];
        }
    }
}