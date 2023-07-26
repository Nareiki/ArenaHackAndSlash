using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBT : BasicTree, IProduct
{
    [SerializeField] private DataBehaviourBase _testData;
    private Blackboard _enemyBlackboard;
    private NavMeshAgent _agent;

    private string _enemyBTName = "BasicEnemy";

    public string ProductName { get => _enemyBTName; private set => _enemyBTName = value; }

    protected override BasicNode SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemyBlackboard = new Blackboard(_testData.GetAttributes(_agent, transform));

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
    }
}
