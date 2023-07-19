using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Cinemachine.Utility;

public class TaskSurroundTarget : BasicNode
{
    private Transform _transform;
    private NavMeshAgent _agent;
    private float _distanceToKeep;
    private float _timer = 0f;
    private float _maxTimeForDirection;
    private float _timeRangeMin, _timeRangeMax;
    private Quaternion _angle;



    public TaskSurroundTarget(Transform transform, NavMeshAgent agent, float distanceToKeep, float timeRangeMin, float timeRangeMax, float angle)
    {
        _transform = transform;
        _agent = agent;
        _distanceToKeep = distanceToKeep;
        _timeRangeMin = timeRangeMin;
        _timeRangeMax = timeRangeMax;
        _maxTimeForDirection = Random.Range(timeRangeMin, timeRangeMax);
        _angle = Quaternion.Euler(0f, angle, 0f);
    }


    public override NodeState Evaluate()
    {
        if (_timer > _maxTimeForDirection)
        {
            _timer = 0f;
            _maxTimeForDirection = Random.Range(_timeRangeMin, _timeRangeMax);
            _angle = Quaternion.Inverse(_angle);
        }
        //TODO: Blackboard
        Transform target = (Transform)GetData("target");

        Vector3 direction = (_transform.position - target.position).normalized;
        Vector3 newPosition = target.position + _angle * direction * _distanceToKeep;

        _agent.destination = newPosition;//Vector3.Slerp(_transform.position, newPosition, timer);

        _timer += Time.deltaTime;

        return State = NodeState.SUCCESS;
    }
}

