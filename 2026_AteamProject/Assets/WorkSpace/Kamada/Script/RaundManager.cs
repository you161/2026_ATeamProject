using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaundManager : MonoBehaviour
{
    [SerializeField] private TestEfects testEfects = null;
    [SerializeField] private float fallPositionY = 0;
    [SerializeField] private GameObject[] raundOverUI = null;

    [Header("ラウンド終了")]
    [SerializeField] private float raundOverTime = 3.0f;

    [Header("フェード")]
    [SerializeField] private FadeManager fadeManager = null;
    [SerializeField] private PlayerManager playerManager = null;

    private bool isRoundOver = false;

    private void Start()
    {
        for (int i = 0; i < raundOverUI.Length; i++)
        {
            raundOverUI[i].SetActive(false);
        }
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

        StartCoroutine(RaundOverCoroutine());
    }

    private IEnumerator RaundOverCoroutine()
    {
        //勝利表示を数秒間見せる
        yield return new WaitForSeconds(raundOverTime);
        //フェードアウト開始
        StartCoroutine(fadeManager.FadeOut());
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

        // ここでプレイヤーの位置をリセット
        playerManager.ResetPlayerPositions();
        isRoundOver = false;
    }
}