using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private int _minEnemyLoaded;

    [SerializeField] private EnemyFactory _enemyFactory;

    private List<EnemyBT> _enemyBTs;

    #region Monobehaviour Methods

    private void OnEnable()
    {
        MSEnemySpawn.Subscribe<BasicEnemy>(RemoveInactiveEnemys);
    }

    private void OnDisable()
    {
        MSEnemySpawn.Unsubscribe<BasicEnemy>(RemoveInactiveEnemys);
    }

    private void Start()
    {
        _enemyBTs = new List<EnemyBT>();
    }

    private void Update()
    {
        SpawnEnemyRandomly();
    }

    #endregion

    #region Enemy Spawning

    private void SpawnEnemyRandomly()
    {
        if (ShouldSpawnEnemy())
        {
            int pointToSpawn = UnityEngine.Random.Range(0, _spawnPoints.Length);
            Vector3 spawnPosition = _spawnPoints[pointToSpawn].position;
            SpawnEnemy(spawnPosition);
        }
    }

    private bool ShouldSpawnEnemy()
    {
        return _enemyBTs.Count < _minEnemyLoaded;
    }

    private void SpawnEnemyForAllSpawnPoints()
    {
        foreach (var point in _spawnPoints)
        {
            SpawnEnemy(point.position);
        }
    }

    private void SpawnEnemy(Vector3 position)
    {
        var enemy = (EnemyBT)_enemyFactory.SpawnObject(position);
        _enemyBTs.Add(enemy);
    }
    #endregion

    #region List Modifications

    private void RemoveInactiveEnemys(BasicEnemy message)
    {
        //enemyBTs.Remove(message);
        _enemyBTs.RemoveAll(enemy => !enemy.isActiveAndEnabled);
    }
    #endregion
}
