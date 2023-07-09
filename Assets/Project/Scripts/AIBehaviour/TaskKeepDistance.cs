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

    private float radiusToMove = 0f;



    public TaskKeepDistance(NavMeshAgent agent, Transform transform, float distance)
    {
        _agent = agent;
        _transform = transform;
        _distanceToKeep = distance;
        _distanceWithBuffer = _distanceToKeep + 1f;
    }



    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (target != null)
        {
            radiusToMove = Vector3.Distance(_transform.position, target.position);


            if (radiusToMove <= _distanceWithBuffer)
            {
                //Calculate direction and distance to move around the target
                Vector3 direction = (_transform.position - target.position).normalized;
                Vector3 newPosition = target.position + direction * _distanceToKeep;

                _agent.destination = newPosition;

                //Debug.Log(" old Position " + _transform.position + "\n| target position " + target.position + "\n| new Position " + newPosition + "\n agent Destination " + _agent.destination);

                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;
    }

}
