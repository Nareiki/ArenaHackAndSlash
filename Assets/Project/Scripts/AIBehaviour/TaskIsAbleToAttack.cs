using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskIsAbleToAttack : BasicNode
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
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _lastTarget = target;
        }

        //TODO: Fix bug with setting Data
        object canAttackObj = GetData("canAttack");

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
            return NodeState.FAILURE;
        }

        _attackCounter += Time.deltaTime;

        if (_attackCounter >= _attackTime)
        {
            if (_canAttack)
            {
                return NodeState.SUCCESS;
            }
            if (_onCooldown)
            {
                if (_cooldownCounter <= _cooldownTime)
                {
                    _cooldownCounter += Time.deltaTime;
                    return NodeState.FAILURE;
                }
                else
                {
                    _cooldownCounter = 0f;
                    _attackCounter = 0f;
                    Parent.Parent.SetData("onCooldown", false);
                    return NodeState.RUNNING;
                }
            }
            Parent.Parent.SetData("onCooldown", false);
            Parent.Parent.SetData("canAttack", true);

            Debug.Log("Can Attack");
            return NodeState.SUCCESS;
        }

        Debug.Log("cannot Attack");
        return _canAttack ? NodeState.RUNNING : NodeState.FAILURE;
    }
}
