using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CheckOutOfRange : BasicNode
{
    [NonReflectionValue]
    private Transform _transform;
    [NonReflectionValue]
    private float _outOfRangeCounter;
    [NonReflectionValue]
    private float _outOfRangeDistance;

    private float _outOfRangeTime;
    private float _fovRange;


    public CheckOutOfRange(Blackboard blackboard) : base(blackboard)
    {
        _transform = blackboard.GetData<Transform>("transform");
        _outOfRangeTime = blackboard.GetData<float>("outOfRangeTime");
        _fovRange = blackboard.GetData<float>("fovRange");
        _outOfRangeDistance = _fovRange + blackboard.GetData<float>("fovRangeBuffer");
    }

    public override NodeState Evaluate()
    {
        Transform target = blackboard.GetData<Transform>("target");

        if (target == null)
        {
            return State = NodeState.RUNNING;
        }
        if (Vector3.Distance(_transform.position, target.position) > _outOfRangeDistance)
        {
            blackboard.SetData("outOfRange", true);
        }
        else
        {
            blackboard.SetData("outOfRange", false);
        }

        bool outOfRange = blackboard.GetData<bool>("outOfRange");

        if (outOfRange && target != null)
        {
            _outOfRangeCounter += Time.deltaTime;
        }
        else
        {
            _outOfRangeCounter = 0;
            return State = NodeState.RUNNING;
        }

        if (_outOfRangeCounter > _outOfRangeTime)
        {
            blackboard.ResetData("target");
            //blackboard.SetData("target", null);
            return State = NodeState.FAILURE;
        }

        return State = NodeState.RUNNING;
    }

}
