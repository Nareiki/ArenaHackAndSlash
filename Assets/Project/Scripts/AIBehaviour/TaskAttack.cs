using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : BasicNode
{
    private Transform _lastTarget;

    //TODO: Make an Enemy Manager
    private BasicEnemy _enemyManager;

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemyManager = target.GetComponent<BasicEnemy>();
            _lastTarget = target;
        }
        else if (target == null)
        {
            return NodeState.FAILURE;
        }

        object canAttackObj = GetData("canAttack");

        if (canAttackObj == null) return NodeState.FAILURE;

        bool canAttack = (bool)canAttackObj;

        if (canAttack)
        {
            Debug.Log("Attack");
            bool? enemyIsDead = _enemyManager?.TakeHit();
            if (enemyIsDead.HasValue && enemyIsDead.Value)
            {
                ClearData("target");
            }

            Parent.Parent.SetData("canAttack", false);
            Parent.Parent.SetData("onCooldown", true);
            return NodeState.SUCCESS;
        }

        State = NodeState.RUNNING;
        return State;
    }
}
