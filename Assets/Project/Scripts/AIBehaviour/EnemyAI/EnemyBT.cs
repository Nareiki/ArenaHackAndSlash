using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBT : BasicTree, IProduct
{

    private NavMeshAgent _agent;
    private Transform _selectedTarget;
    private LayerMask _enemyLayerMask = 1 << 15 | 1 << 17;

    [SerializeField] private float _distanceToKeep = 6f;

    [SerializeField] public Transform[] _waypoints;
    [SerializeField] public float _minRange = 1f;
    [SerializeField] public float _maxRange = 5f;

    private string _enemyBTName = "BasicEnemy";
    public string ProductName { get => _enemyBTName; private set => _enemyBTName = value; }

    protected override BasicNode SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.stoppingDistance = 1.5f;

        BasicNode root = new Selector(new List<BasicNode>
        {
            //new Sequence(new List<BasicNode>
            //{
            //    new CheckEnemyInAttackRange(transform),
            //    //new TaskAttack(),
                

            //}),
            new Sequence(new List<BasicNode> {
                new CheckEnemyInAttackRange(transform),
                new TaskAttack()
            }),
            new Sequence(new List<BasicNode> {
                    new TaskIsAbleToAttack(),
                    new TaskGoToTarget(transform, _agent),
            }),
            new Sequence(new List<BasicNode>
            {
                new CheckEnemyInFOVRange(transform, _enemyLayerMask),
                new TaskKeepDistance(_agent, transform, _distanceToKeep),
                new TaskSurroundTarget(transform, _agent, _distanceToKeep, _minRange, _maxRange),
                //new TaskGoToTarget(transform, _agent),
            }),
            new TaskPatrol(transform, _waypoints, _agent),
        });
        return root;

    }

    public void Initialize()
    {
        gameObject.name = _enemyBTName;
    }
}
