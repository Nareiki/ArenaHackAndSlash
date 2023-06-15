using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : BasicNode
{
    private Transform _lastTarget;

    //TODO: Make an Enemy Manager
    private EnemyManager _enemyManager;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;


    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemyManager = target.GetComponent<EnemyManager>();
            _lastTarget = target;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            bool enemyIsDead = _enemyManager.TakeHit();
            if (enemyIsDead)
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
