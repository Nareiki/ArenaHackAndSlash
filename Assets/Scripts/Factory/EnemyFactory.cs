using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class EnemyFactory : Factory, IFactory
{

    //private Dictionary<string, Type> _enemys;

    [SerializeField] private GameObject _basicEnemy;

    public GameObject ObjectToSpawn { get => _basicEnemy; }

    [SerializeField] private Transform[] _waypointArray;

    public IProduct SpawnObject(Vector3 position)
    {
        return GetEnemy(position);
    }

    public override IProduct GetEnemy(Vector3 position)
    {
        //var enemyTypes = Assembly.GetAssembly(typeof(BasicEnemy).GetTypes());
        // create a Prefab instance and get the product component
        GameObject inst = Instantiate(_basicEnemy, position, Quaternion.identity);
        EnemyBT enemy = inst.GetComponent<EnemyBT>();

        // each product contains its own logic
        enemy.Initialize();

        enemy._waypoints = _waypointArray;

        return enemy;
    }


}

