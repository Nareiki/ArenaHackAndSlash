using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskGoToTarget : BasicNode
{
    [NonReflectionValue]
    private Transform _transform;
    [NonReflectionValue]
    private NavMeshAgent _agent;
    private float _targetReachedRange;

    public TaskGoToTarget(Blackboard blackboard) : base(blackboard)
    {
        _transform = blackboard.GetData<Transform>("transform");
        _agent = blackboard.GetData<NavMeshAgent>("agent");
        _targetReachedRange = blackboard.GetData<float>("targetReachedRange");
    }

    public override NodeState Evaluate()
    {
        Transform target = blackboard.GetData<Transform>("target");

        if (target == null)
        {
            blackboard.ResetData("target");
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
