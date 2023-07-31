using UnityEngine;

[CreateAssetMenu(fileName = "New GuardBehaviourData", menuName = "BehaviourTreeData/GuardBehaviourData", order = 1)]
public class GuardBehaviourData : DataBehaviourBase
{
	public float attackRange = 0f;
	public LayerMask enemyLayerMask;
	public float fovRange = 0f;
	public float fovRangeBuffer = 0f;
	public float targetReachedRange = 0f;
	public Transform[] waypoints;
	public float waitTimePerPoint = 0f;
}