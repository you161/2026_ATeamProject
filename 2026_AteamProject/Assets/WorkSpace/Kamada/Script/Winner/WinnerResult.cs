using UnityEngine;

public class WinnerResult : MonoBehaviour
{
    [SerializeField] private WinnerData winnerData = null;
    [SerializeField] private GameObject[] playerObject = null;
    [SerializeField] private CameraMover cameraMover = null;
    private Coroutine resultCoroutine;
    private int winnerNumber = 0;
    private void Start()
    {
        resultCoroutine = null;

        for (int i = 0; i < playerObject.Length; i++)
        {
            if(i == winnerData.WinnerNumber)
            {
                playerObject[i].SetActive(true);
                winnerNumber = i;
                continue;
            }
            playerObject[i].SetActive(false);
        }

        if (resultCoroutine != null)
        {
            return;
        }
        else
        {
            resultCoroutine = StartCoroutine(cameraMover.ResultEffects(winnerNumber));
            Debug.Log("a");
        }
    }
}