using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private readonly int FallHash = Animator.StringToHash("Falling");
    private const float CrossFadeDuration = 0.1f;

    public PlayerFallState(PlayerStateMachine stateMashine) : base(stateMashine) { }

    public override void Enter()
    {
        stateMachine.Velocity.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }

    public override void Tick()
    {
        //ApplyGravity();
        Move();

        //if (stateMachine.Controller.isGrounded)
        //{
        //    stateMachine.SwitchState(new PlayerMoveState(stateMachine));

        //}
    }

    public override void Exit() { }
}
