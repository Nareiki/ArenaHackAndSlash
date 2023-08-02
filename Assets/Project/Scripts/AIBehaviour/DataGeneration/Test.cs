using UnityEngine;

[CreateAssetMenu(fileName = "New Test", menuName = "BehaviourTreeData/Test", order = 1)]
public class Test : DataBehaviourBase
{
	public float attackRange = 2.5f;
	public LayerMask enemyLayerMask;
	public float fovRange = 6f;
	public float fovRangeBuffer = 2f;
	public float targetReachedRange = 2f;
	public Transform[] waypoints;
	public float waitTimePerPoint = 2f;
}