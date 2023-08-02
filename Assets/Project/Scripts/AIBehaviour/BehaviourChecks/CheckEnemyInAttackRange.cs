using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInAttackRange : BasicNode
{
    [NonReflectionValue]
    private Transform _transform;
    private float _attackRange = 1.5f;

    public CheckEnemyInAttackRange(Blackboard blackboard) : base(blackboard)
    {
        _transform = blackboard.GetData<Transform>("transform");
        _attackRange = blackboard.GetData<float>("attackRange");
    }

    public override NodeState Evaluate()
    {
        Transform target = blackboard.GetData<Transform>("target");
        if (target == null)
        {
            return State = NodeState.FAILURE;
        }

        if (Vector3.Distance(_transform.position, target.position) <= _attackRange)
        {
            bool test = blackboard.GetData<bool>("canAttack");
            if (!test)
                return State = NodeState.FAILURE;

            return State = NodeState.SUCCESS;
        }

        return State = NodeState.FAILURE;
    }
}
