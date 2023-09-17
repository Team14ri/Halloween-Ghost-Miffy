using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerController player;
    private StateMachine stateMachine;

    public PlayerIdleState(PlayerController player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public void Enter() { /* 초기화 코드 */ }

    public void Execute()
    {
        Debug.Log($"Idle {player.movementInput}");
        
        if (player.movementInput != Vector2.zero)
        {
            stateMachine.ChangeState(new PlayerWalkingState(player, stateMachine));
        }
    }

    public void Exit() { /* 정리 코드 */ }
}