using UnityEngine;

public class WinnerResult : MonoBehaviour
{
    [SerializeField] private WinnerData winnerData = null;
    [SerializeField] private GameObject[] playerObject = null;
    private void Start()
    {
        for(int i = 0; i < playerObject.Length; i++)
        {
            if(i == winnerData.WinnerNumber)
            {
                playerObject[i].SetActive(true);
                continue;
            }
            playerObject[i].SetActive(false);
        }
    }
}