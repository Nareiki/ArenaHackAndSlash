using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerStateMachine : StateMachine
{
    public Vector3 Velocity;
    public float MovementSpeed { get; private set; } = 5f;
    public float JumpForce { get; private set; } = 5f;
    public float LookRotationDampFactor { get; private set; } = 10f;
    public Transform MainCamera { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    private void Start()
    {
        MainCamera = Camera.main.transform;

        PlayerInput = GetComponent<PlayerInput>();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();

        SwitchState(new PlayerMoveState(this));
    }
}
