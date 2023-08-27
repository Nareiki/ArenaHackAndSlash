using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : ScriptableObject
{
    [SerializeField] private GameObject _basicEnemy;

    public IProduct SpawnObject(Vector3 position)
    {
        return GetEnemy(position);
    }

    public IProduct GetEnemy(Vector3 position)
    {
        //var enemyTypes = Assembly.GetAssembly(typeof(BasicEnemy).GetTypes());
        // create a Prefab instance and get the product component
        #region create not pooled enemy
        //GameObject inst = Instantiate(_basicEnemy, position, Quaternion.identity);
        //EnemyBT enemy = inst.GetComponent<EnemyBT>();
        //// each product contains its own logic
        //enemy.Initialize();

        //enemy._waypoints = _waypointArray;
        //return pooledEnemyScript;

        #endregion

        #region create pooled Enemy
        GameObject pooledEnemy = ObjectPooler.SharedInstance.GetPooledObject("Enemy");
        if (pooledEnemy != null)
        {
            pooledEnemy.transform.position = position;
            pooledEnemy.transform.rotation = Quaternion.identity;
            pooledEnemy.SetActive(true);
        }
        BasicEnemy pooledEnemyLogic = pooledEnemy.GetComponent<BasicEnemy>();
        pooledEnemyLogic.Initialize();
        EnemyBT pooledEnemyScript = pooledEnemy.GetComponent<EnemyBT>();
        //pooledEnemyScript.Initialize(_waypointArray);

        #endregion

        return pooledEnemyScript;
    }
}
