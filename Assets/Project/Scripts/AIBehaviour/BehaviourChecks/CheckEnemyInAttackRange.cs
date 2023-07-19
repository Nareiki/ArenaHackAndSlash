using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInAttackRange : BasicNode
{
    private Transform _transform;
    private float _attackRange = 1.5f;

    public CheckEnemyInAttackRange(Transform transform, float attackRange)
    {
        _transform = transform;
        _attackRange = attackRange;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            return State = NodeState.FAILURE;
        }

        Transform target = (Transform)t;
        if (Vector3.Distance(_transform.position, target.position) <= _attackRange)
        {
            bool test = (bool)GetData("canAttack");
            if (!test)
                return State = NodeState.FAILURE;

            return State = NodeState.SUCCESS;
        }

        return State = NodeState.FAILURE;
    }
}
