using System;
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
            cameraForward * player.MovementInput.y + 
            cameraRight * player.MovementInput.x;

        moveDirection.Normalize(); // 방향 벡터 정규화

        Vector3 moveVelocity = moveDirection * player.data.MovementSpeed;
        player.Rb.velocity = new Vector3(moveVelocity.x, player.Rb.velocity.y, moveVelocity.z);
        
        // 만약 움직임 입력이 없으면 Idle 상태로 전환
        if (player.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
            return;
        }

        if (player.MovementInput.x == 0f)
            return;

        float newScaleX = MathF.Abs(player.model.transform.localScale.x);
        newScaleX *= player.MovementInput.x < 0f ? -1f : 1f;
        
        player.model.transform.localScale = new Vector3(
            newScaleX, 
            player.model.transform.localScale.y, 
            player.model.transform.localScale.z);
    }

    public void Exit()
    {
        // Todo: 이동 애니메이션 정지 등 정리
    }
}
