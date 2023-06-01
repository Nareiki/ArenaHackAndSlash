using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardBT : BasicTree
{

    private NavMeshAgent _agent;
    public Transform[] Waypoints;

    public static float Speed = 2f;

    protected override BasicNode SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();
        BasicNode root = new TaskPatrol(transform, Waypoints, _agent);
        return root;
    }
}
