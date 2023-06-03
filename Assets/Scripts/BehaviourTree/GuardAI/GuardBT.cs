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

    public static float FovRange = 6f;

    public static float AttackRange = 1f;

    protected override BasicNode SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();
        BasicNode root = new Selector(new List<BasicNode>
        {
            new Sequence(new List<BasicNode>{
                new CheckEnemyInAttackRange(transform),
                new TaskAttack(),
            }),
            new Sequence(new List<BasicNode>{
                new CheckEnemyInFOVRange(transform),
                new TaskGoToTarget(transform, _agent),

            }),
            new TaskPatrol(transform, Waypoints, _agent),
        });
        //BasicNode root = new TaskPatrol(transform, Waypoints, _agent);
        return root;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, GuardBT.FovRange);
    }
}
