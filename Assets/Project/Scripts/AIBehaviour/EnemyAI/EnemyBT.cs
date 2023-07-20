using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBT : BasicTree, IProduct
{
    private Blackboard _enemyBlackboard = new Blackboard();

    private NavMeshAgent _agent;
    [SerializeField] private LayerMask _enemyLayerMask = 1 << 15 | 1 << 17;
    [Header("Patrol Waypoints")]
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
    [Header("Time the enemy takes to loose interest")]
    [SerializeField] private float _outOfRangeTime = 2f;

    private string _enemyBTName = "BasicEnemy";

    public string ProductName { get => _enemyBTName; private set => _enemyBTName = value; }

    protected override BasicNode SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();

        //Initialize Required Data
        _enemyBlackboard.SetData("transform", transform);
        _enemyBlackboard.SetData("agent", _agent);
        _enemyBlackboard.SetData("enemyLayerMask", _enemyLayerMask);
        _enemyBlackboard.SetData("waypoints", _waypoints);
        _enemyBlackboard.SetData("minRange", _minRange);
        _enemyBlackboard.SetData("maxRange", _maxRange);
        _enemyBlackboard.SetData("angle", _angle);
        _enemyBlackboard.SetData("attackRange", _attackRange);
        _enemyBlackboard.SetData("targetReachedRange", _targetReachedRange);
        _enemyBlackboard.SetData("fovRange", _fovRange);
        _enemyBlackboard.SetData("rangeBuffer", _rangeBuffer);
        _enemyBlackboard.SetData("distanceToKeep", _distanceToKeep);
        _enemyBlackboard.SetData("waitTimePerPoint", _waitTimePerPoint);
        _enemyBlackboard.SetData("outOfRangeTime", _outOfRangeTime);

        //Initialized Shared Data
        _enemyBlackboard.SetData("target", null);
        _enemyBlackboard.SetData("canAttack", false);
        _enemyBlackboard.SetData("onCooldown", false);
        _enemyBlackboard.SetData("outOfRange", false);

        BasicNode root = new Selector(new List<BasicNode>
        {
            new Sequence(new List<BasicNode> {
                new CheckEnemyInAttackRange(_enemyBlackboard),
                new TaskAttack(_enemyBlackboard)
            }),
            new Sequence(new List<BasicNode> {
                new CheckOutOfRange(_enemyBlackboard),
                new CheckIsAbleToAttack(_enemyBlackboard),
                new TaskGoToTarget(_enemyBlackboard),
            }),
            new Sequence(new List<BasicNode>
            {
                new CheckEnemyInFOVRange(_enemyBlackboard),
                new TaskKeepDistance(_enemyBlackboard),
                new TaskSurroundTarget(_enemyBlackboard),
            }),
            new TaskPatrol(_enemyBlackboard),
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
