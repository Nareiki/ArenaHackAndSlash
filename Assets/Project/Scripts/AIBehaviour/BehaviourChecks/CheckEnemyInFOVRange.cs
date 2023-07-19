using System.Transactions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInFOVRange : BasicNode
{
    private Transform _transform;
    private LayerMask _enemyLayerMask;
    private float _fovRange;

    private float _fovRangeBuffer;

    public CheckEnemyInFOVRange(Transform transform, LayerMask enemyLayerMask, float fovRange, float fovRangeBuffer)
    {
        _transform = transform;
        _enemyLayerMask = enemyLayerMask;
        _fovRange = fovRange;
        _fovRangeBuffer = fovRangeBuffer;
    }

    public override NodeState Evaluate()
    {
        //TODO: Blackboard
        object t = GetData("target");
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _fovRange, _enemyLayerMask);
            if (colliders.Length > 0)
            {
                //TODO: Blackbord
                Parent.Parent.SetData("target", colliders[0].transform);
                Parent.Parent.SetData("canAttack", false);
                return State = NodeState.SUCCESS;
            }

            return State = NodeState.FAILURE;
        }
        Transform target = (Transform)t;
        if (Vector3.Distance(_transform.position, target.position) > _fovRange + _fovRangeBuffer)
        {
            //TODO: Blackbord
            ClearData("target");
            return State = NodeState.FAILURE;
        }

        return State = NodeState.SUCCESS;
    }
}
