using System;
using UnityEngine;

public class PlayerMovingState : IState
{
    private PlayerController player;
    private StateMachine stateMachine;

    private Animator animator;
    
    private readonly int IdleState;
    private readonly int MovingState;

    private float maxSlopeAngle = 75f;

    public PlayerMovingState(PlayerController player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        animator = player.model.GetComponent<Animator>();
        
        IdleState = Animator.StringToHash("Miffy@Idle");
        MovingState = Animator.StringToHash("Miffy@Move_Connect");
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
        SetAnimation(MovingState);
    }
    
    private const float RAY_DISTANCE = 1.8f;
    private RaycastHit slopeHit;
    private int groundLayer = 1 << LayerMask.NameToLayer("Ground");  // 땅(Ground) 레이어만 체크
    
    public bool IsOnSlope()
    {
        if (Physics.Raycast(player.transform.position, Vector3.down, out slopeHit, 
                RAY_DISTANCE, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlopeAngle;
        }
        return false;
    }
    
    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
    
    private float CalculateNextFrameGroundAngle(Vector3 moveVector)
    {
        var nextFramePlayerPosition =
            player.transform.position + moveVector * Time.fixedDeltaTime;

        if (Physics.Raycast(nextFramePlayerPosition, Vector3.down, out RaycastHit hitInfo,
                RAY_DISTANCE, groundLayer))
        {
            // 월드의 위쪽 방향과 바닥의 노멀 벡터 사이의 각도 계산
            float groundAngle = Vector3.Angle(Vector3.up, hitInfo.normal);

            return groundAngle;
        }
        return 0f;
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

        Vector3 moveVelocity = CalculateNextFrameGroundAngle(moveDirection * player.data.MovementSpeed) < maxSlopeAngle ?
            moveDirection : Vector3.zero;
        Vector3 gravity = Vector3.down * Mathf.Abs(player.Rb.velocity.y);
        
        player.Rb.useGravity = true;
        if (IsOnSlope())
        {
            moveVelocity = AdjustDirectionToSlope(moveVelocity);
            gravity = Vector3.zero;
            player.Rb.useGravity = false;
        }
        
        player.Rb.velocity = moveVelocity * player.data.MovementSpeed + gravity;
        
        // 만약 움직임 입력이 없으면 Idle 상태로 전환
        if (player.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
            return;
        }

        if (player.MovementInput.x == 0f)
            return;
        
        player.ChangePlayerFacing(player.MovementInput.x < 0f ? -1 : 1);
    }

    public void Exit()
    {
        SetAnimation(IdleState);
    }
}
