using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardBT : BasicTree
{

    private NavMeshAgent _agent;
    [SerializeField] private Transform[] _waypoints;

    public static float Speed = 2f;

    public static float FovRange = 6f;

    public static float AttackRange = 2f;

    private LayerMask _enemyLayerMask = 1 << 16;

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
                new CheckEnemyInFOVRange(transform, _enemyLayerMask),
                new TaskGoToTarget(transform, _agent),

            }),
            new TaskPatrol(transform, _waypoints, _agent),
        });
        //BasicNode root = new TaskPatrol(transform, Waypoints, _agent);
        return root;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, GuardBT.FovRange);
    }
}
