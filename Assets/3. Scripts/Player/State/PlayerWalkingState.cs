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
        // 카메라의 방향에서 Y축을 제외한 방향 벡터
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraRight = Camera.main.transform.right;

        // 카메라 방향 기준으로 움직임 방향을 계산
        Vector3 moveDirection = 
            cameraForward * player.movementInput.y + 
            cameraRight * player.movementInput.x;

        moveDirection.Normalize(); // 방향 벡터 정규화

        Vector3 moveVelocity = moveDirection * 3.5f; // 하드 코딩 수정 필요: moveSpeed
        player.rb.velocity = new Vector3(moveVelocity.x, player.rb.velocity.y, moveVelocity.z); // 플레이어의 Rigidbody에 속도 적용

        
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
