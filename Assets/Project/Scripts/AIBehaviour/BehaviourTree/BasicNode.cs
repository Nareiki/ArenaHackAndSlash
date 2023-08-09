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

[System.Serializable]
public class BasicNode : INode
{
    [NonReflectionValue]
    protected NodeState State;

    [SerializeField]
    public BasicNode Parent;
    [SerializeField]
    [NonReflectionValue]
    protected List<BasicNode> Children = new List<BasicNode>();
    [SerializeField]
    public virtual NodeState Evaluate() => NodeState.FAILURE;
    [NonReflectionValue]
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
