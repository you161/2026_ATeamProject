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
        int gamepadCount = Gamepad.all.Count;

        //接続されているGamePad数と最大人数の小さい方
        int playerCount = Mathf.Min(gamepadCount,maxPlayerCount);
        playerCount = Mathf.Min(playerCount,spawnPoints.Length);

        Debug.Log($"GamePad数 : {gamepadCount}");
        Debug.Log($"Player生成数 : {playerCount}");

        for (int i = 0; i < playerCount; i++)
        {
            CreatePlayer(i);
        }
    }

    private void CreatePlayer(int playerIndex)
    {
        Gamepad gamepad = Gamepad.all[playerIndex];

        PlayerInput playerInput = PlayerInput.Instantiate(
            playerPrefab,
            playerIndex: playerIndex,
            controlScheme: null,
            splitScreenIndex: -1,
            pairWithDevice: gamepad
        );

        playerInput.transform.position = spawnPoints[playerIndex].position;
        playerInput.transform.rotation = spawnPoints[playerIndex].rotation;
    }
}