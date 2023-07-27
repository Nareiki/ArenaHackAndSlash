using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "New MyData", menuName = "BehaviourTreeData/MyData", order = 1)]
public class MyData : DataBehaviourBase
{
    public System.Single attackRange = 1.5f;
    public System.Single outOfRangeTime = 2;
    public System.Single fovRange = 6;
    public System.Single attackSpeed = 1;
    public System.Single cooldownTime = 5;
    public System.Single targetReachedRange = 1.5f;
    public UnityEngine.LayerMask enemyLayerMask;
    public System.Single fovRangeBuffer = 6;
    public System.Single distanceToKeep = 6;
    public System.Single minRange = 0;
    public System.Single maxRange = 0;
    public System.Single angle;
    public UnityEngine.Transform[] waypoints;
    public System.Single waitTimePerPoint = 1.5f;

}