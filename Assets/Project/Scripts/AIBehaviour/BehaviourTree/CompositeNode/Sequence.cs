using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : BasicNode
{
    public Sequence() : base() { }

    public Sequence(List<BasicNode> children) : base(children) { }

    public override NodeState Evaluate()
    {
        bool anyChildIsRunning = false;

        foreach (BasicNode node in Children)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    State = NodeState.FAILURE;
                    return State;
                case NodeState.SUCCESS:
                    State = NodeState.SUCCESS;
                    continue;
                case NodeState.RUNNING:
                    State = NodeState.RUNNING;
                    continue;
                default:
                    State = NodeState.SUCCESS;
                    return State;
            }
        }

        State = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;

        return State;
    }
}
