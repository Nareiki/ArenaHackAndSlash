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

    private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

    public BasicNode()
    {
        Parent = null;

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

    public void SetData(string key, object value)
    {
        _dataContext[key] = value;
    }

    public object GetData(string key)
    {
        object val = null;

        if (_dataContext.TryGetValue(key, out val))
            return val;

        BasicNode node = Parent;

        if (Parent != null)
            val = node.GetData(key);

        return val;
    }

    public bool ClearData(string key)
    {
        bool cleared = false;
        if (_dataContext.ContainsKey(key))
        {
            _dataContext.Remove(key);
            return true;
        }

        BasicNode node = Parent;

        if (node != null)
            cleared = node.ClearData(key);

        return cleared;
    }
}
