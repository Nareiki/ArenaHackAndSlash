using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}

public class BasicNode
{
    protected NodeState State;

    public BasicNode Parent;

    protected List<BasicNode> Children = new List<BasicNode>();

    public virtual NodeState Evaluate() => NodeState.FAILURE;

    protected Blackboard blackboard;

    public BasicNode()
    {
        Parent = null;
    }

    public BasicNode(Blackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public BasicNode(List<BasicNode> children)
    {
        foreach (BasicNode child in children)
        {
            Attach(child);
        }

    }

    private void Attach(BasicNode node)
    {
        node.Parent = this;
        Children.Add(node);
    }
}
