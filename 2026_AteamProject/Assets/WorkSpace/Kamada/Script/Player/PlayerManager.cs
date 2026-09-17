using UnityEngine;
using UnityEngine.InputSystem;

public class GamePadPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Transform[] spawnPoints = null;
    [SerializeField] private int maxPlayerCount = 2;

    private void Start()
    {
        CreatePlayers();
    }

    private void CreatePlayers()
    {
        //接続されているゲームパッドの数を取得
        int gamepadCount = Gamepad.all.Count;
        //最大プレイヤー数とスポーンポイントの数を考慮して、生成するプレイヤー数を決定
        int playerCount = Mathf.Min(maxPlayerCount,spawnPoints.Length);;

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

        //操作方法を決定
        PlayerControllerInput controllerInput =
            playerInput.GetComponent<PlayerControllerInput>();

        if (playerIndex == 0)
        {
            //P1
            if (playerIndex < gamepadCount)
            {
                controllerInput.SetInputType(PlayerControllerInput.InputType.GamePad);
            }
            else
            {
                controllerInput.SetInputType(PlayerControllerInput.InputType.WASD);
            }
        }
        else
        {
            //P2
            if (playerIndex < gamepadCount)
            {
                controllerInput.SetInputType(PlayerControllerInput.InputType.GamePad);
            }
            else
            {
                controllerInput.SetInputType(PlayerControllerInput.InputType.Arrow);
            }
        }

        // スポーン位置
        playerInput.transform.position = spawnPoints[playerIndex].position;
        playerInput.transform.rotation = spawnPoints[playerIndex].rotation;
    }
}