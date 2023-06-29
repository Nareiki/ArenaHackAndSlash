using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PlayerBaseState : State
{
    protected readonly PlayerStateMachine stateMachine;

    protected PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void CalculateMoveDirection()
    {
        Vector3 cameraForward = new(stateMachine.MainCamera.forward.x, 0, stateMachine.MainCamera.forward.z);
        Vector3 cameraRight = new(stateMachine.MainCamera.right.x, 0, stateMachine.MainCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * stateMachine.PlayerInput.MoveVal.y + cameraRight.normalized * stateMachine.PlayerInput.MoveVal.x;

        //stateMachine.Rigidbody.velocity = new Vector3(moveDirection.x * stateMachine.MovementSpeed, moveDirection.y * stateMachine.MovementSpeed, stateMachine.Rigidbody.velocity.z);
        stateMachine.Velocity.x = moveDirection.x * stateMachine.MovementSpeed;
        stateMachine.Velocity.z = moveDirection.z * stateMachine.MovementSpeed;

        //Vector3.forward * moveDirection.x * stateMachine.MovementSpeed;
        //stateMachine.Rigidbody.velocity = Vector3.right* moveDirection.z * stateMachine.MovementSpeed;
    }

    protected void FaceMoveDirection()
    {
        Vector3 faceDirection = new(stateMachine.Velocity.x, 0f, stateMachine.Velocity.z);

        if (faceDirection == Vector3.zero)
            return;

        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), stateMachine.LookRotationDampFactor * Time.deltaTime);
    }

    // protected void ApplyGravity()
    // {
    //     if (stateMachine.Velocity.y > stateMachine.Rigidbody.mass * gravity)
    //     {
    //         //stateMachine.Velocity.y -= stateMachine.Rigidbody.mass * gravity * Time.deltaTime;
    //         stateMachine.Rigidbody.velocity = Vector3.down * stateMachine.Rigidbody.mass * gravity * Time.deltaTime;
    //     }
    // }

    protected void Move()
    {
        //Debug.Log(stateMachine.PlayerInput.moveVal);
        //Debug.Log(stateMachine.Velocity);

        stateMachine.Rigidbody.velocity = stateMachine.Velocity;// * Time.deltaTime * movementSpeed; //new Vector3(stateMachine.PlayerInput.moveVal.x * movementSpeed * Time.deltaTime, stateMachine.Velocity.y, stateMachine.PlayerInput.moveVal.y * movementSpeed * Time.deltaTime);
        //Debug.Log(stateMachine.Rigidbody.velocity);
        //stateMachine.Controller.Move(stateMachine.Velocity * Time.deltaTime);
    }
}
