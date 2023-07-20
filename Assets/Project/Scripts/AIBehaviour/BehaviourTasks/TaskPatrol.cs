using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskPatrol : BasicNode
{

    private Transform _transform;

    private Transform[] _waypoints;

    private NavMeshAgent _agent;

    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _waiting = false;

    public TaskPatrol(Blackboard blackboard) : base(blackboard)
    {
        _transform = blackboard.GetData<Transform>("transform");
        _waypoints = blackboard.GetData<Transform[]>("waypoints");
        _agent = blackboard.GetData<NavMeshAgent>("agent");
        _waitTime = blackboard.GetData<float>("waitTimePerPoint");
    }

    public override NodeState Evaluate()
    {
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
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
