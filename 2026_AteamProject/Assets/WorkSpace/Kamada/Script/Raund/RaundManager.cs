using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaundManager : MonoBehaviour
{
    [SerializeField] private TestEfects testEfects = null;
    [SerializeField] private float fallPositionY = 0;
    [SerializeField] private GameObject[] raundOverUI = null;

    [Header("ラウンド終了")]
    [SerializeField] private float raundOverTime = 3.0f;
    [Header("勝利ラウンド数")]
    [SerializeField] private int maxCount = 3;

    [Header("フェード")]
    [SerializeField] private FadeManager fadeManager = null;
    [Header("プレイヤーマネージャー")]
    [SerializeField] private PlayerManager playerManager = null;
    [Header("カウントダウン")]
    [SerializeField] private CountDown countDown = null;
    [Header("シーンマネージャー")]
    [SerializeField] private GameSceneManager gameSceneManager = null;
    [SerializeField] private TestEfects testEffects = null;
    [SerializeField] private WinnerData winnerData = null;
    [SerializeField] private SEManager seManager = null;

    private bool isRoundOver = false;
    public bool IsRoundOver { get => isRoundOver; }
    private bool isEnd = false;

    private void Start()
    {
        for (int i = 0; i < raundOverUI.Length; i++)
        {
            raundOverUI[i].SetActive(false);
        }

        isRoundOver = false;
        isEnd = false;

        winnerData.WinnerNumber = 0;
    }

    private void Update()
    {
        CheckRoundOver();
    }

    private void CheckRoundOver()
    {
        if (isRoundOver)
        {
            return;
        }

        //カウントダウン中は落下判定しない
        if (countDown != null && countDown.IsPlayCount && !countDown.IsGo)
        {
            return;
        }

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObject in playerObjects)
        {
            if (playerObject == null)
            {
                continue;
            }

            //落下しているか
            if (playerObject.transform.position.y > fallPositionY)
            {
                continue;
            }

            PlayerInput playerInput = playerObject.GetComponent<PlayerInput>();

            if (playerInput == null)
            {
                continue;
            }

            isRoundOver = true;

            RaundOver(playerInput.playerIndex);

            break;
        }
    }

    private void RaundOver(int playerNum)
    {
        if (playerNum == 0)
        {
            //P1が落ちた → P2の勝利
            testEfects.WinnerCountP2();
            raundOverUI[1].SetActive(true);
        }
        else if (playerNum == 1)
        {
            //P2が落ちた → P1の勝利
            testEfects.WinnerCountP1();
            raundOverUI[0].SetActive(true);
        }

        seManager.RoundOverSE();

        StartCoroutine(RaundOverCoroutine());
    }

    private IEnumerator RaundOverCoroutine()
    {
        //勝利表示を数秒間見せる
        yield return new WaitForSeconds(raundOverTime);

        if (testEffects.CountP1 >= maxCount || testEffects.CountP2 >= maxCount)
        {
            if(testEffects.CountP1 > testEffects.CountP2)
            {
                winnerData.WinnerNumber = 0;
            }
            else
            {
                winnerData.WinnerNumber = 1;
            }

            if (!isEnd)
            {
                gameSceneManager.LoadResaultScene();
                isEnd = true;
            }
        }

        //フェードアウト開始
        if (!isEnd)
        {
            StartCoroutine(fadeManager.FadeOut());
        }

        //フェードが終わるまで待つ
        yield return new WaitUntil(() => !fadeManager.GetIsFading());

        //画面が完全に黒くなった
        Restart();
        //次のラウンドを表示
        StartCoroutine(fadeManager.FadeIn());
    }

    private void Restart()
    {
        for (int i = 0; i < raundOverUI.Length; i++)
        {
            raundOverUI[i].SetActive(false);
        }

        //プレイヤーを初期位置へ戻す
        playerManager.ResetPlayerPositions();
        //ラウンド終了状態を解除
        isRoundOver = false;
        //再スタート
        countDown.StartCountDown();
    }
}