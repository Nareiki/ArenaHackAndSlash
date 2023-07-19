using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOutOfRange : BasicNode
{

    private float _outOfRangeTime;
    private float _outOfRangeCounter;


    public CheckOutOfRange(float outOfRangeTime)
    {
        _outOfRangeTime = outOfRangeTime;
    }

    public override NodeState Evaluate()
    {
        //TODO: Blackboard
        object targetObj = GetData("target");
        //TODO: Blackboard
        object outOfRangeObj = GetData("outOfRange");
        if (outOfRangeObj == null)
            return State = NodeState.RUNNING;

        bool outOfRange = (bool)outOfRangeObj;

        if (outOfRange && targetObj != null)
        {
            _outOfRangeCounter += Time.deltaTime;
        }
        else
        {
            _outOfRangeCounter = 0;
        }

        if (_outOfRangeCounter > _outOfRangeTime)
        {
            ClearData("target");
            return State = NodeState.RUNNING;
        }

        return State = NodeState.RUNNING;
    }

}
