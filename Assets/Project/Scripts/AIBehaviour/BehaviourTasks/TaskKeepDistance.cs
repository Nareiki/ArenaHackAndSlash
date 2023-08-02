using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskKeepDistance : BasicNode
{
    [NonReflectionValue]
    private Transform _transform;
    [NonReflectionValue]
    private NavMeshAgent _agent;
    [NonReflectionValue]
    private float _distanceWithBuffer;
    [NonReflectionValue]
    private float _radiusToMove = 0f;

    private float _distanceToKeep;

    public TaskKeepDistance(Blackboard blackboard) : base(blackboard)
    {
        _agent = blackboard.GetData<NavMeshAgent>("agent");
        _transform = blackboard.GetData<Transform>("transform");
        _distanceToKeep = blackboard.GetData<float>("distanceToKeep");
        _distanceWithBuffer = _distanceToKeep + blackboard.GetData<float>("fovRangeBuffer");
    }

    public override NodeState Evaluate()
    {
        Transform target = blackboard.GetData<Transform>("target");

        if (target != null)
        {
            _radiusToMove = Vector3.Distance(_transform.position, target.position);

            if (_radiusToMove <= _distanceWithBuffer)
            {
                //Calculate direction and distance to move around the target
                Vector3 direction = (_transform.position - target.position).normalized;
                Vector3 newPosition = target.position + direction * _distanceToKeep;

                _agent.destination = newPosition;

                return State = NodeState.SUCCESS;
            }
        }

        return State = NodeState.FAILURE;
    }

}
