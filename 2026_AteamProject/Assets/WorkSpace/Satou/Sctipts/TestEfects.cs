using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TestEfects : MonoBehaviour
{
    [SerializeField] private List<Animator> winnerAnimaP1 = new List<Animator>();
    [SerializeField] private List<Animator> winnerAnimaP2 = new List<Animator>();
    [SerializeField] private List<GameObject> stockP1 = new List<GameObject>();
    [SerializeField] private List<GameObject> stockP2 = new List<GameObject>();

    [SerializeField] private int winnerP1 = 0;
    [SerializeField] private int winnerP2 = 0;
    public int CountP1 { get => winnerP1; }
    public int CountP2 { get => winnerP2; }

    private void Start()
    {
        winnerP1 = 0;
        winnerP2 = 0;
    }

    //P1勝利時のUI変更処理
    public void WinnerCountP1()
    {
        if (winnerP1 < winnerAnimaP1.Count)
        {
            winnerAnimaP1[winnerP1].SetTrigger("Winner");
            stockP1[winnerP1].SetActive(true);
            winnerP1++;
            //Debug.Log("WinnerP1 Stock " + winnerP1);
        }
    }
    //P2勝利時のUI変更処理
    public void WinnerCountP2()
    {
        if (winnerP2 < winnerAnimaP2.Count)
        {
            winnerAnimaP2[winnerP2].SetTrigger("Winner");
            stockP2[winnerP2].SetActive(true);
            winnerP2++;
            //Debug.Log("WinnerP2 Stock " + winnerP2);
        }

    }
}