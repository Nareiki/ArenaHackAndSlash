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


    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _lastTarget = target;
        }
        if (target == null)
        {
            Parent.Parent.Parent.ClearData("canAttack");
        }

        _attackCounter += Time.deltaTime;

        //TODO: Fix bug with setting Data
        _canAttack = (bool)GetData("canAttack") ? true : false;

        if (_canAttack)
        {
            return NodeState.RUNNING;
        }

        if (_attackCounter >= _attackTime)
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
            }
            Parent.Parent.Parent.SetData("canAttack", true);
            Debug.Log("Can Attack");
            return NodeState.SUCCESS;
        }


        Debug.Log("cannot Attack");
        return NodeState.RUNNING;
    }
}
