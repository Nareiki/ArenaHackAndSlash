using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : BasicNode
{
    private Transform _lastTarget;
    private IDamageable _enemyManager;

    public override NodeState Evaluate()
    {
        //TODO: Blackbord
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemyManager = target.GetComponent<BasicEnemy>();
            _lastTarget = target;
        }
        else if (target == null)
        {
            return State = NodeState.FAILURE;
        }

        // TODO: Blackbord
        object canAttackObj = GetData("canAttack");

        if (canAttackObj == null) return State = NodeState.FAILURE;

        bool canAttack = (bool)canAttackObj;

        if (canAttack)
        {
            Debug.Log("Attack");
            bool? enemyIsDead = _enemyManager?.TakeHit();
            if (enemyIsDead.HasValue && enemyIsDead.Value)
            {
                ClearData("target");
            }

            //TODO: Blackbord
            Parent.Parent.SetData("canAttack", false);
            Parent.Parent.SetData("onCooldown", true);
            return State = NodeState.SUCCESS;
        }

        return State = NodeState.RUNNING;
    }
}
