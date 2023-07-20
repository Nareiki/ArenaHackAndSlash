using UnityEngine;

public class CheckEnemyInFOVRange : BasicNode
{
    private Transform _transform;
    private LayerMask _enemyLayerMask;
    private float _fovRange;

    private float _fovRangeBuffer;

    public CheckEnemyInFOVRange(Blackboard blackboard) : base(blackboard)
    {
        _transform = blackboard.GetData<Transform>("transform");
        _enemyLayerMask = blackboard.GetData<LayerMask>("enemyLayerMask");
        _fovRange = blackboard.GetData<float>("fovRange");
        _fovRangeBuffer = blackboard.GetData<float>("rangeBuffer");
    }

    public override NodeState Evaluate()
    {
        Transform target = blackboard.GetData<Transform>("target");
        if (target == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _fovRange, _enemyLayerMask);
            if (colliders.Length > 0)
            {
                blackboard.SetData("target", colliders[0].transform);
                blackboard.SetData("canAttack", false);
                return State = NodeState.SUCCESS;
            }

            return State = NodeState.FAILURE;
        }
        if (Vector3.Distance(_transform.position, target.position) > _fovRange + _fovRangeBuffer)
        {
            blackboard.ResetData("target");
            return State = NodeState.FAILURE;
        }

        return State = NodeState.SUCCESS;
    }
}
