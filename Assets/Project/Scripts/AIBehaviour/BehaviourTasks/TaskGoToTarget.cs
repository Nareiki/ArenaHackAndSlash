using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskGoToTarget : BasicNode
{
    private Transform _transform;
    private NavMeshAgent _agent;
    private float _targetReachedRange;

    public TaskGoToTarget(Transform transform, NavMeshAgent agent, float targetReachedRange)
    {
        _transform = transform;
        _agent = agent;
        _targetReachedRange = targetReachedRange;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (target == null)
        {
            ClearData("target");
            return State = NodeState.FAILURE;
        }

        if (Vector3.Distance(_transform.position, target.position) > _targetReachedRange)
        {
            _agent.destination = target.position;
        }
        State = NodeState.RUNNING;
        return State;
    }
}
