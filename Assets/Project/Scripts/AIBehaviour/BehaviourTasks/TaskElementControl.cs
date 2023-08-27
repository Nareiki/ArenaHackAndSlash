using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class TaskElementControl : BasicNode
{
    private Elements _currentElement;
    [NonReflectionValue]
    private float _speed;
    [NonReflectionValue]
    private NavMeshAgent _agent;
    [NonReflectionValue]
    private IDamageable _thisBody;
    [NonReflectionValue]
    private float _timeCounter;
    [NonReflectionValue]
    private float _maxTime;

    public TaskElementControl(Blackboard blackboard) : base(blackboard)
    {
        _currentElement = blackboard.GetData<Elements>("element");
        _agent = blackboard.GetData<NavMeshAgent>("agent");
        _speed = _agent.speed;
        _thisBody = blackboard.GetData<Transform>("transform").GetComponent<IDamageable>();
    }

    public override NodeState Evaluate()
    {
        _currentElement = blackboard.GetData<Elements>("element");
        switch (_currentElement)
        {
            case Elements.None:
                _agent.speed = _speed;
                _timeCounter = 0;
                return State = NodeState.FAILURE;
            case Elements.Fire:

                _maxTime = 1;
                _agent.speed = _speed * 1.5f;
                return State = DamageOverTime(5f); ; 
            case Elements.Water:

                _maxTime = 2.5f;
                _agent.speed = _speed * 0.5f;
                return State = DamageOverTime(10f); ;
            case Elements.Toxic:
                _agent.speed = _speed * 0.5f;
                break;
            default:
                return State = NodeState.FAILURE;
        }

        return State = NodeState.FAILURE;
    }

    private NodeState DamageOverTime(float damage)
    {

        if(_timeCounter > _maxTime)
        {
            bool? isDead = _thisBody.TakeHit(damage);
            if (isDead.HasValue && isDead.Value)
            {
                return NodeState.SUCCESS;
            }
            _timeCounter = 0;
        }

        _timeCounter += Time.deltaTime;

        return State = NodeState.FAILURE;
    }
}
