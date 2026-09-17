using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Transform[] spawnPoints = null;
    [SerializeField] private int maxPlayerCount = 2;

    private PlayerInput[] players;

    private void Start()
    {
        players = new PlayerInput[maxPlayerCount];
        CreatePlayers();
    }

    private void CreatePlayers()
    {
        //接続されているゲームパッドの数を取得
        int gamepadCount = Gamepad.all.Count;

        //最大プレイヤー数とスポーンポイントの数を考慮して、生成するプレイヤー数を決定
        int playerCount = Mathf.Min(maxPlayerCount, spawnPoints.Length);

        for (int i = 0; i < playerCount; i++)
        {
            CreatePlayer(i, gamepadCount);
        }
    }

    private void CreatePlayer(int playerIndex, int gamepadCount)
    {
        PlayerInput playerInput;

        //ゲームパッドがある場合
        if (playerIndex < gamepadCount)
        {
            Gamepad gamepad = Gamepad.all[playerIndex];

            playerInput = PlayerInput.Instantiate(
                playerPrefab,
                playerIndex: playerIndex,
                controlScheme: null,
                splitScreenIndex: -1,
                pairWithDevice: gamepad
            );
        }
        //ゲームパッドがない場合
        else
        {
            playerInput = PlayerInput.Instantiate(playerPrefab,playerIndex: playerIndex);
        }

        //プレイヤーを保存
        players[playerIndex] = playerInput;
        //操作方法を決定
        PlayerControllerInput controllerInput = playerInput.GetComponent<PlayerControllerInput>();

        if (playerIndex == 0)
        {
            //P1
            if (playerIndex < gamepadCount)
            {
                controllerInput.SetInputType(
                    PlayerControllerInput.InputType.GamePad
                );
            }
            else
            {
                controllerInput.SetInputType(
                    PlayerControllerInput.InputType.WASD
                );
            }
        }
        else
        {
            //P2
            if (playerIndex < gamepadCount)
            {
                controllerInput.SetInputType(
                    PlayerControllerInput.InputType.GamePad
                );
            }
            else
            {
                controllerInput.SetInputType(
                    PlayerControllerInput.InputType.Arrow
                );
            }
        }

        //スポーン位置
        playerInput.transform.position = spawnPoints[playerIndex].position;
        playerInput.transform.rotation = spawnPoints[playerIndex].rotation;
    }

    //プレイヤーの位置をスポーン位置にリセット
    public void ResetPlayerPositions()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                continue;
            }

            players[i].transform.position = spawnPoints[i].position;
            players[i].transform.rotation = spawnPoints[i].rotation;
        }
    }
}