using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private readonly int MoveSpeedHash = Animator.StringToHash("MovementSpeed");
    private readonly int MoveBlendTreeHash = Animator.StringToHash("MoveBlendTree");
    private const float AnimationDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //stateMachine.Velocity.y = Physics.gravity.y;

        stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);

        //stateMachine.PlayerInput.OnJumpPerformed += SwitchToJumpState;
    }

    public override void Tick()
    {
        //Debug.Log(stateMachine.Controller.isGrounded);
        //if (!stateMachine.Controller.isGrounded)
        //{
        //stateMachine.SwitchState(new PlayerFallState(stateMachine));
        //}

        CalculateMoveDirection();
        FaceMoveDirection();
        Move();


        stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.PlayerInput.MoveVal.sqrMagnitude > 0f ? 1f : 0f, AnimationDampTime, Time.deltaTime);
    }

    public override void Exit()
    {
        //stateMachine.PlayerInput.OnJumpPerformed -= SwitchToJumpState;
    }

    private void SwitchToJumpState()
    {
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }
}
