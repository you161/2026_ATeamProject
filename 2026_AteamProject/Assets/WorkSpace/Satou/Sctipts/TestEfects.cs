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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Keyboard.current.enterKey.wasPressedThisFrame)
        //{
        //    WinnerCountP1();
        //}
        //else if(Keyboard.current.spaceKey.wasPressedThisFrame)
        //{
        //    WinnerCountP2();
        //}
    }

    //P1勝利時のUI変更処理
    public void WinnerCountP1()
    {
        if (winnerP1 < winnerAnimaP1.Count)
        {
            winnerAnimaP1[winnerP1].SetTrigger("Winner");
            stockP1[winnerP1].SetActive(true);
            winnerP1++;
            Debug.Log("WinnerP1 Stock " + winnerP1);
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
            Debug.Log("WinnerP2 Stock " + winnerP2);
        }

    }
}
