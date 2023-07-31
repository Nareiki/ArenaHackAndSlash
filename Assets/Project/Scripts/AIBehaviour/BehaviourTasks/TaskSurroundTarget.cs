using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Cinemachine.Utility;

public class TaskSurroundTarget : BasicNode
{
    [NonReflectionValue]
    private Transform _transform;
    [NonReflectionValue]
    private NavMeshAgent _agent;
    [NonReflectionValue]
    private float _timer = 0f;
    [NonReflectionValue]
    private float _maxTimeForDirection;
    [NonReflectionValue]
    private Quaternion _angleQuat;

    private float _distanceToKeep;
    private float _minRange, _rangeMax;
    private float _angle;



    public TaskSurroundTarget(Blackboard blackboard) : base(blackboard)
    {
        _transform = blackboard.GetData<Transform>("transform");
        _agent = blackboard.GetData<NavMeshAgent>("agent");
        _distanceToKeep = blackboard.GetData<float>("distanceToKeep");
        _minRange = blackboard.GetData<float>("minRange");
        _rangeMax = blackboard.GetData<float>("maxRange");
        _maxTimeForDirection = Random.Range(_minRange, _rangeMax);
        _angle = blackboard.GetData<float>("angle");
        _angleQuat = Quaternion.Euler(0f, _angle, 0f);
    }


    public override NodeState Evaluate()
    {
        if (_timer > _maxTimeForDirection)
        {
            _timer = 0f;
            _maxTimeForDirection = Random.Range(_minRange, _rangeMax);
            _angleQuat = Quaternion.Inverse(_angleQuat);
        }

        //TODO: Blackboard
        Transform target = blackboard.GetData<Transform>("target");

        Vector3 direction = (_transform.position - target.position).normalized;
        Vector3 newPosition = target.position + _angleQuat * direction * _distanceToKeep;

        _agent.destination = newPosition;//Vector3.Slerp(_transform.position, newPosition, timer);

        _timer += Time.deltaTime;

        return State = NodeState.SUCCESS;
    }
}

