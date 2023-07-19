using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIsAbleToAttack : BasicNode
{
    private Transform _lastTarget;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;

    private float _cooldownTime = 5f;
    private float _cooldownCounter = 0f;

    private bool _canAttack = false;
    private bool _onCooldown = false;


    public override NodeState Evaluate()
    {
        //TODO: Blackboard
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _lastTarget = target;
        }

        //TODO: Blackboard
        object canAttackObj = GetData("canAttack");
        //TODO: Blackboard
        object onCooldownObj = GetData("onCooldown");

        if (onCooldownObj != null)
        {
            _onCooldown = (bool)onCooldownObj;
        }

        if (canAttackObj != null)
        {
            _canAttack = (bool)canAttackObj;
        }
        else
        {
            return State = NodeState.FAILURE;
        }

        _attackCounter += Time.deltaTime;

        if (_attackCounter >= _attackTime)
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
                    //TODO: Blackboard
                    Parent.Parent.SetData("onCooldown", false);
                    return State = NodeState.RUNNING;
                }
            }
            //TODO: Blackboard
            Parent.Parent.SetData("onCooldown", false);
            Parent.Parent.SetData("canAttack", true);

            return State = NodeState.SUCCESS;
        }

        return _canAttack ? State = NodeState.RUNNING : State = NodeState.FAILURE;
    }
}
