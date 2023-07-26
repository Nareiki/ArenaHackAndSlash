using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIsAbleToAttack : BasicNode
{
    [NonReflectionValue]
    private Transform _lastTarget;
    [NonReflectionValue]
    private float _attackCounter = 0f;
    [NonReflectionValue]
    private float _cooldownCounter = 0f;
    [NonReflectionValue]
    private bool _canAttack = false;
    [NonReflectionValue]
    private bool _onCooldown = false;

    private float _attackSpeed = 1f;
    private float _cooldownTime = 5f;


    public CheckIsAbleToAttack(Blackboard blackboard) : base(blackboard)
    {
        _attackSpeed = blackboard.GetData<float>("attackSpeed");
        _cooldownTime = blackboard.GetData<float>("cooldownTime");
    }

    public override NodeState Evaluate()
    {
        Transform target = blackboard.GetData<Transform>("target");
        if (target != _lastTarget)
        {
            _lastTarget = target;
        }

        _canAttack = blackboard.GetData<bool>("canAttack");
        _onCooldown = blackboard.GetData<bool>("onCooldown");

        _attackCounter += Time.deltaTime;

        if (_attackCounter >= _attackSpeed)
        {
            if (_canAttack)
            {
                return State = NodeState.SUCCESS;
            }
            if (_onCooldown)
            {
                if (_cooldownCounter <= _cooldownTime)
                {
                    _cooldownCounter += Time.deltaTime;
                    return State = NodeState.FAILURE;
                }
                else
                {
                    _cooldownCounter = 0f;
                    _attackCounter = 0f;
                    blackboard.SetData("onCooldown", false);
                    return State = NodeState.RUNNING;
                }
            }
            blackboard.SetData("onCooldown", false);
            blackboard.SetData("canAttack", true);
            return State = NodeState.SUCCESS;
        }

        return _canAttack ? State = NodeState.RUNNING : State = NodeState.FAILURE;
    }
}
