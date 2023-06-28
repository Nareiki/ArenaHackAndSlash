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

    [SerializeField] private EnemyFactoryScr _enemyFactoryScr;

    private void Start()
    {

        SpawnEnemyRandomly();

    }
    private void Update()
    {
        foreach (var enemy in enemyBTs)
        {
            print(enemy.ProductName);
            if (!enemy.isActiveAndEnabled)
            {
                enemyBTs.Remove(enemy);
            }
        }
    }

    private void SpawnEnemyRandomly()
    {
        foreach (var point in _spawnPoints)
        {
            var enemy = (EnemyBT)_enemyFactoryScr.SpawnObject(point.position);
            enemyBTs.Add(enemy);
        }
    }
}
