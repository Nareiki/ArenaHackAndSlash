using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : BasicNode
{
    [NonReflectionValue]
    private Transform _lastTarget;
    [NonReflectionValue]
    private IDamageable _enemyManager;

    public TaskAttack(Blackboard blackboard) : base(blackboard) { }

    public override NodeState Evaluate()
    {
        Transform target = blackboard.GetData<Transform>("target");// (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemyManager = target.GetComponent<IDamageable>();
            _lastTarget = target;
        }
        else if (target == null)
        {
            return State = NodeState.FAILURE;
        }

        object canAttackObj = blackboard.GetData<bool>("canAttack"); // GetData("canAttack");

        if (canAttackObj == null) return State = NodeState.FAILURE;

        bool canAttack = (bool)canAttackObj;

        if (canAttack)
        {
            bool? enemyIsDead = _enemyManager?.TakeHit();
            if (enemyIsDead.HasValue && enemyIsDead.Value)
            {
                blackboard.ResetData("target");
            }

            blackboard.SetData("canAttack", false);
            blackboard.SetData("onCooldown", true);
            return State = NodeState.SUCCESS;
        }

        return State = NodeState.RUNNING;
    }
}
