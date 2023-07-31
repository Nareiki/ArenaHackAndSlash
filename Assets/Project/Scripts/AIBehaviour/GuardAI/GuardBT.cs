using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardBT : BasicTree
{
    [SerializeField] private DataBehaviourBase _guardData;
    private Blackboard _guardBlackboard;
    private NavMeshAgent _agent;

    protected override BasicNode SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();
        _guardBlackboard = new Blackboard(_guardData.GetAttributes(_agent, transform));

        BasicNode root = new Selector(new List<BasicNode>
        {
            new Sequence(new List<BasicNode>{
                new CheckEnemyInAttackRange(_guardBlackboard),
                new TaskAttack(_guardBlackboard),
            }),
            new Sequence(new List<BasicNode>{
                new CheckEnemyInFOVRange(_guardBlackboard),
                new TaskGoToTarget(_guardBlackboard),

            }),
            new TaskPatrol(_guardBlackboard),
        });
        return root;
    }
}
