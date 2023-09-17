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

    public void Enter()
    {
        
    }
    
    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}