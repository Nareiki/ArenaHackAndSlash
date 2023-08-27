using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyBehaviourData", menuName = "BehaviourTreeData/EnemyBehaviourData", order = 1)]
public class EnemyBehaviourData : DataBehaviourBase
{
	public Elements currentElement;
	public float attackRange = 1.5f;
	public float outOfRangeTime = 2f;
	public float fovRange = 6f;
	public float attackSpeed = 1f;
	public float cooldownTime = 5f;
	public float targetReachedRange = 1.5f;
	public LayerMask enemyLayerMask;
	public float fovRangeBuffer = 6f;
	public float distanceToKeep = 6f;
	public float minRange = 1f;
	public float rangeMax = 5f;
	public float angle = 20f;
	public Transform[] waypoints;
	public float waitTimePerPoint = 1.5f;
}