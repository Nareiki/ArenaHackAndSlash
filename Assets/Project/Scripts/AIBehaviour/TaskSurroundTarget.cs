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
    private float _rangeMin, _rangeMax;
    private Quaternion angle;



    public TaskSurroundTarget(Transform transform, NavMeshAgent agent, float distanceToKeep, float rangeMin, float rangeMax)
    {
        _transform = transform;
        _agent = agent;
        _distanceToKeep = distanceToKeep;
        _rangeMin = rangeMin;
        _rangeMax = rangeMax;
        _maxTimeForDirection = Random.Range(rangeMin, rangeMax);
        angle = Quaternion.Euler(0f, 20, 0f);
    }


    public override NodeState Evaluate()
    {
        if (_timer > _maxTimeForDirection)
        {
            _timer = 0f;
            _maxTimeForDirection = Random.Range(_rangeMin, _rangeMax);
            angle = Quaternion.Inverse(angle);
        }

        Transform target = (Transform)GetData("target");

        Vector3 direction = (_transform.position - target.position).normalized;
        Vector3 newPosition = target.position + angle * direction * _distanceToKeep;

        //Debug.DrawLine(direction, direction + Vector3.up, Color.red, 10f);
        //Debug.DrawLine(newPosition, newPosition + Vector3.up, Color.green, 10f);


        _agent.destination = newPosition;//Vector3.Slerp(_transform.position, newPosition, timer);


        _timer += Time.deltaTime;

        return NodeState.SUCCESS;
    }
}

