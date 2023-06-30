using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskIsAbleToAttack : BasicNode
{
    private Transform _lastTarget;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;


    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _lastTarget = target;
        }

        _attackCounter += Time.deltaTime;

        if (_attackCounter >= _attackTime)
        {
            Debug.Log("Can Attack");
            return NodeState.SUCCESS;
        }
        else
        {

            _attackCounter = 0f;
        }


        Debug.Log("cannot Attack");
        return NodeState.RUNNING;
    }
}
