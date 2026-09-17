using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private enum PlayerState
    {
        Idle = 0,
        Walk = 1,
        Dive = 2,
        PickUp = 3,
        Throw = 4
    }

    [SerializeField] private Animator animator = null;
    private static readonly int PlayerHash = Animator.StringToHash("Player");
    [SerializeField] private PlayerMove playerMove = null;
    [SerializeField] private PlayerJump playerJump = null;
    [SerializeField] private Bomb bomb = null;
    private PlayerState currentState = PlayerState.Idle;
    private bool currentIsExplosion = false;
    private void Start()
    {
        currentState = PlayerState.Idle;
    }
    private void Update()
    {
        if (bomb.IsExplosion)
        {
            currentIsExplosion = false;
        }

        CheckState();
    }
    private void CheckState()
    {
        if (bomb.IsReady && !currentIsExplosion)
        {
            if (!bomb.IsThrow)
            {
                ChangeState(PlayerState.PickUp);
            }
            else
            {
                if (!currentIsExplosion)
                {
                    ChangeState(PlayerState.Throw);
                    currentIsExplosion = true;
                }
            }
        }
        else if (playerJump.IsJumping)
        {
            ChangeState(PlayerState.Dive);
        }
        else if (playerMove.IsMove)
        {
            ChangeState(PlayerState.Walk);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
    }
    private void ChangeState(PlayerState newState)
    {
        if (currentState == newState)
        {
            return;
        }

        currentState = newState;
        animator.SetInteger(PlayerHash, (int)currentState);
    }
}