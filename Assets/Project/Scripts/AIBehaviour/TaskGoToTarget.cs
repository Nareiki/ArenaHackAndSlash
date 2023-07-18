using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskGoToTarget : BasicNode
{
    private Transform _transform;
    private NavMeshAgent _agent;

    public TaskGoToTarget(Transform transform, NavMeshAgent agent)
    {
        _transform = transform;
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if(target == null)
        {
            ClearData("target");
            return NodeState.FAILURE;
        }

        if (Vector3.Distance(_transform.position, target.position) > 1.5f)
        {
            _agent.destination = target.position;
        }
        State = NodeState.RUNNING;
        return State;
    }
}
