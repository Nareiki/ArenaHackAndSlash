using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardBT : BasicTree
{

    private NavMeshAgent _agent;
    private LayerMask _enemyLayerMask = 1 << 16;

    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _fovRange = 6f;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _rangeBuffer = 2f;
    [SerializeField] private float _targetReachedRange = 1.5f;
    [SerializeField] private float _waitTime = 1f;

    protected override BasicNode SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();

        BasicNode root = new Selector(new List<BasicNode>
        {
            new Sequence(new List<BasicNode>{
                new CheckEnemyInAttackRange(transform, _attackRange),
                new TaskAttack(),
            }),
            new Sequence(new List<BasicNode>{
                new CheckEnemyInFOVRange(transform, _enemyLayerMask, _fovRange, _rangeBuffer),
                new TaskGoToTarget(transform, _agent, _targetReachedRange),

            }),
            new TaskPatrol(transform, _waypoints, _agent, _waitTime),
        });
        return root;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _fovRange);
    }
}
