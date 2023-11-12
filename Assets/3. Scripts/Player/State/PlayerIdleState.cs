using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerController player;
    private StateMachine stateMachine;

    private Animator animator;

    private readonly int IdleState;

    public PlayerIdleState(PlayerController player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        animator = player.model.GetComponent<Animator>();
        
        IdleState = Animator.StringToHash("Miffy@Idle");
    }
    
    private void SetAnimation(int stateID)
    {
        if (animator == null)
            return;
            
        if (!animator.HasState(0, stateID))
        {
            Debug.LogWarning($"{stateID} 애니메이션이 존재하지 않습니다.");
            return;
        }
            
        animator.CrossFade(stateID, 0f);
    }

    public void Enter()
    {
        SetAnimation(IdleState);
    }
    
    public void Execute()
    {
        // Idle -> Moving
        if (player.MovementInput != Vector2.zero)
        {
            stateMachine.ChangeState(new PlayerMovingState(player, stateMachine));
        }
    }

    public void Exit() { /* 초기화 */ }
}