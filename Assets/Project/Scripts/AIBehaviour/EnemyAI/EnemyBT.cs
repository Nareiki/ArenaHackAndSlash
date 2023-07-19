using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBT : BasicTree, IProduct
{

    private NavMeshAgent _agent;
    private LayerMask _enemyLayerMask = 1 << 15 | 1 << 17;

    [SerializeField] private Transform[] _waypoints;
    [Header("Time Range for changing position of Surround")]
    [SerializeField] private float _minRange = 1f;
    [SerializeField] private float _maxRange = 5f;
    [Range(0, 360)]
    [SerializeField] private float _angle = 20;
    [Header("Attack Range")]
    [SerializeField] private float _attackRange = 1.5f;
    [Header("Destination Reched Range")]
    [SerializeField] private float _targetReachedRange = 1.5f;
    [Header("Fov Range")]
    [SerializeField] private float _fovRange = 6f;
    [SerializeField] private float _rangeBuffer = 6f;
    [Header("Distance to keep")]
    [SerializeField] private float _distanceToKeep = 6f;
    [Header("PatrolWaitTime")]
    [SerializeField] private float _waitTimePerPoint = 1.5f;


    private string _enemyBTName = "BasicEnemy";

    public string ProductName { get => _enemyBTName; private set => _enemyBTName = value; }

    protected override BasicNode SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();

        BasicNode root = new Selector(new List<BasicNode>
        {
            new Sequence(new List<BasicNode> {
                new CheckEnemyInAttackRange(transform, _attackRange),
                new TaskAttack()
            }),
            new Sequence(new List<BasicNode> {
                    new CheckIsAbleToAttack(),
                    new TaskGoToTarget(transform, _agent, _targetReachedRange),
            }),
            new Sequence(new List<BasicNode>
            {
                new CheckEnemyInFOVRange(transform, _enemyLayerMask, _fovRange, _rangeBuffer),
                new TaskKeepDistance(_agent, transform, _distanceToKeep, _rangeBuffer),
                new TaskSurroundTarget(transform, _agent, _distanceToKeep, _minRange, _maxRange, _angle),
            }),
            new TaskPatrol(transform, _waypoints, _agent, _waitTimePerPoint),
        });
        return root;
    }

    public void Initialize()
    {
        gameObject.name = _enemyBTName;
    }

    public void Initialize(Transform[] waypoints)
    {
        Initialize();
        _waypoints = waypoints;
    }
}
