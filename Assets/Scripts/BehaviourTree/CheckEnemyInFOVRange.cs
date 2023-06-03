using System.Transactions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInFOVRange : BasicNode
{
    private Transform _transform;
    private static LayerMask _enemyLayerMask = 1 << 16;

    private float _distanceBuffer = 2f;

    public CheckEnemyInFOVRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        //Debug.Log(t);
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, GuardBT.FovRange, _enemyLayerMask);
            Debug.Log(colliders.Length);
            if (colliders.Length > 0)
            {
                Parent.Parent.SetData("target", colliders[0].transform);
                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.FAILURE;
            return State;
        }
        Transform target = (Transform)t;
        if (Vector3.Distance(_transform.position, target.position) > GuardBT.FovRange + _distanceBuffer)
        {
            ClearData("target");
            State = NodeState.FAILURE;
            return State;
        }

        State = NodeState.SUCCESS;
        return State;
    }
}
