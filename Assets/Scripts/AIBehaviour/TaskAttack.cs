using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : BasicNode
{
    private Transform _lastTarget;

    //TODO: Make an Enemy Manager
    private BasicEnemy _enemyManager;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;


    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemyManager = target.GetComponent<BasicEnemy>();
            _lastTarget = target;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            bool? enemyIsDead = _enemyManager.TakeHit();
            if (enemyIsDead.Value)
            {
                ClearData("target");
            }
            else
            {
                _attackCounter = 0f;
            }
        }

        State = NodeState.RUNNING;
        return State;
    }

}
