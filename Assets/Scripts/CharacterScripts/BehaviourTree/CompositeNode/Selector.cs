using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BasicNode
{
    public Selector() : base() { }

    public Selector(List<BasicNode> children) : base(children) { }

    public override NodeState Evaluate()
    {
        foreach (BasicNode node in Children)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    State = NodeState.FAILURE;
                    continue;
                case NodeState.SUCCESS:
                    State = NodeState.SUCCESS;
                    return State;
                case NodeState.RUNNING:
                    State = NodeState.RUNNING;
                    return State;
                default:
                    continue;
            }
        }

        State = NodeState.FAILURE;

        return State;
    }
}
