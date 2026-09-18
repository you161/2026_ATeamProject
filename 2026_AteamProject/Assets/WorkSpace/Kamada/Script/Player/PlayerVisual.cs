using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Renderer renderer = null;
    [SerializeField] private Material player1Material = null;
    [SerializeField] private Material player2Material = null;

    public void SetPlayerNumber(int playerIndex)
    {
        Material material = playerIndex == 0 ? player1Material : player2Material;
        renderer.material = material;
    }
}