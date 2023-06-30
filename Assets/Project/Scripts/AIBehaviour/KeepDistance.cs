using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KeepDistance : BasicNode
{
    private NavMeshAgent _agent;
    private Transform _transform;
    private float _distanceToKeep;



    public KeepDistance(NavMeshAgent agent, Transform transform, float distance)
    {
        _agent = agent;
        _transform = transform;
        _distanceToKeep = distance;
    }



    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        float angle = Vector3.Angle(_transform.position, target.position);

        if (Vector3.Distance(_transform.position, target.position) < _distanceToKeep)
        {
            _agent.isStopped = true;

            Vector3 newPosition = _transform.position;

            //TODO: Google or use ChatGPT to look up a way to use angle and magnitude to calculate a new Vector


            return NodeState.SUCCESS;
        }
        _agent.isStopped = false;


        return NodeState.FAILURE;
    }

}
