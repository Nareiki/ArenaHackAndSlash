using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIsAbleToAttack : BasicNode
{
    private Transform _lastTarget;

    private float _attackSpeed = 1f;
    private float _attackCounter = 0f;

    private float _cooldownTime = 5f;
    private float _cooldownCounter = 0f;

    private bool _canAttack = false;
    private bool _onCooldown = false;

    public CheckIsAbleToAttack(Blackboard blackboard) : base(blackboard)
    {
        _attackSpeed = blackboard.GetData<float>("attackSpeed");
        _cooldownTime = blackboard.GetData<float>("cooldownTime");
        _canAttack = blackboard.GetData<bool>("canAttack");
        _onCooldown = blackboard.GetData<bool>("onCooldown");
    }

    public override NodeState Evaluate()
    {
        Transform target = blackboard.GetData<Transform>("target");
        if (target != _lastTarget)
        {
            _lastTarget = target;
        }

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
