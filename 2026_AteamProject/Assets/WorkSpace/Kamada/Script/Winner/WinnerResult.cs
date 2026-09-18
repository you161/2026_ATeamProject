using UnityEngine;

public class WinnerResult : MonoBehaviour
{
    [SerializeField] private WinnerData winnerData = null;
    [SerializeField] private GameObject[] playerObject = null;
    [SerializeField] private CameraMover cameraMover = null;
    private void Start()
    {
        for(int i = 0; i < playerObject.Length; i++)
        {
            if(i == winnerData.WinnerNumber)
            {
                playerObject[i].SetActive(true);
                StartCoroutine(cameraMover.ResultEffects(i));
                continue;
            }
            playerObject[i].SetActive(false);
        }
    }
}