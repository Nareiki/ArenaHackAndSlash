using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;

    [SerializeField] private GameObject _enemy;


    [SerializeField] private EnemyFactory _enemyFactory;

    [SerializeField] private List<EnemyBT> enemyBTs;

    private void Start()
    {

        SpawnEnemyRandomly();

    }
    private void Update()
    {
        foreach (var enemy in enemyBTs)
        {
            print(enemy.ProductName);
        }
    }

    private void SpawnEnemyRandomly()
    {
        foreach (var point in _spawnPoints)
        {
            var enemy = (EnemyBT)_enemyFactory.SpawnObject(point.position);
            enemyBTs.Add(enemy);
        }
    }
}
