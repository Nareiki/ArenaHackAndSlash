using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class DataBehaviourBase : ScriptableObject
{
    protected Type thisType;
    public NavMeshAgent agent;


    public virtual Dictionary<string, object> GetAttributes(NavMeshAgent agent, Transform transform)
    {
        thisType = this.GetType();
        this.agent = agent;
        Dictionary<string, object> tempDict = new Dictionary<string, object>
        {
            { "transform", transform }
        };

        var fields = thisType.GetFields();

        foreach (var field in fields)
        {
            //Debug.Log(field.Name + "  " + field.GetValue(this));
            tempDict.Add(field.Name, field.GetValue(this));
        }
        return tempDict;
    }
}