using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskPatrol : BasicNode
{
    [NonReflectionValue]
    private Transform _transform;
    [NonReflectionValue]
    private NavMeshAgent _agent;
    [NonReflectionValue]
    private int _currentWaypointIndex = 0;
    private Transform[] _waypoints;
    // Time to wait when reaching waypoints
    private float _waitTimePerPoint = 1f;
    [NonReflectionValue]
    private float _waitCounter = 0f;
    [NonReflectionValue]
    private bool _waiting = false;

    public TaskPatrol(Blackboard blackboard) : base(blackboard)
    {
        _transform = blackboard.GetData<Transform>("transform");
        _waypoints = blackboard.GetData<Transform[]>("waypoints");
        _agent = blackboard.GetData<NavMeshAgent>("agent");
        _waitTimePerPoint = blackboard.GetData<float>("waitTimePerPoint");
    }

    public override NodeState Evaluate()
    {
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTimePerPoint)
            {
                _waiting = false;
            }
        }
        else
        {
            Transform wp = _waypoints[_currentWaypointIndex];
            if (Vector3.Distance(_transform.position, wp.position) < 1f)
            {
                //_transform.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
            else
            {
                _agent.destination = wp.position;
                /*_transform.position = Vector3.MoveTowards(_transform.position, wp.position, GuardBT.Speed * Time.deltaTime);
                _transform.LookAt(wp.position);*/
            }
        }

        return State = NodeState.RUNNING;
    }
}
