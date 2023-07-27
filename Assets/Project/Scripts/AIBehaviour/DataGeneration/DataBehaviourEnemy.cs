using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "BehaviourDataEnemy", menuName = "BehaviourTreeData/Enemy")]
public class DataBehaviourEnemy : DataBehaviourBase
{
    public LayerMask enemyLayerMask = 1 << 15 | 1 << 17;
    [Header("Patrol Waypoints")]
    public Transform[] waypoints;
    [Header("Time Range for changing position of Surround")]
    public float minRange = 1f;
    public float maxRange = 5f;
    [Range(0, 360)]
    public float angle = 20;
    [Header("Attack Range")]
    public float attackRange = 1.5f;
    [Header("Destination Reched Range")]
    public float targetReachedRange = 1.5f;
    [Header("Fov Range")]
    public float fovRange = 6f;
    public float fovRangeBuffer = 6f;
    [Header("Distance to keep")]
    public float distanceToKeep = 6f;
    [Header("PatrolWaitTime")]
    public float waitTimePerPoint = 1.5f;
    [Header("Time the enemy takes to loose interest")]
    public float outOfRangeTime = 2f;
    [Header("Attack Speed of BT")]
    public float attackSpeed = 1f;
    [Header("Cooldown Time between attacks")]
    public float cooldownTime = 5f;
}

