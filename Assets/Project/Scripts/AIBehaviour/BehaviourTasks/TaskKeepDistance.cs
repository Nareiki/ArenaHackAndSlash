using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskKeepDistance : BasicNode
{
    private NavMeshAgent _agent;
    private Transform _transform;
    private float _distanceToKeep;
    private float _distanceWithBuffer;
    private float _radiusToMove = 0f;

    public TaskKeepDistance(Blackboard blackboard) : base(blackboard)
    {
        _agent = blackboard.GetData<NavMeshAgent>("agent");
        _transform = blackboard.GetData<Transform>("transform");
        _distanceToKeep = blackboard.GetData<float>("distanceToKeep");
        _distanceWithBuffer = _distanceToKeep + blackboard.GetData<float>("rangeBuffer");
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
