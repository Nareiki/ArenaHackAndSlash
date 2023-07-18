using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInAttackRange : BasicNode
{
    private Transform _transform;

    public CheckEnemyInAttackRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            State = NodeState.FAILURE;
            return State;
        }

        Transform target = (Transform)t;
        if (Vector3.Distance(_transform.position, target.position) <= 1.5f)
        {
            bool test = (bool)GetData("canAttack");
            if (!test) return NodeState.FAILURE;

            //TODO: Do the Attacking
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}
