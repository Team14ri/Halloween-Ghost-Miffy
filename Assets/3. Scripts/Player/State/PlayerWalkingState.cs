using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : IState
{
    private PlayerController player;
    private StateMachine stateMachine;

    public PlayerWalkingState(PlayerController player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        // Todo: 이동 애니메이션 시작 등 초기화
    }

    public void Execute()
    {
        Debug.Log($"Walking {player.movementInput}");
        
        // Todo: 이동 처리

        // 만약 움직임 입력이 없으면 Idle 상태로 전환
        if (player.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
        }
    }

    public void Exit()
    {
        // Todo: 이동 애니메이션 정지 등 정리
    }
}
