using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;

    private IDamageable[] _enemies;

    [SerializeField] private GameObject _enemy;
    [SerializeField] private Transform[] _waypoint;

    private void Start()
    {

        _enemies = FindObjectsOfType<BasicEnemy>();// FindAnyObjectByType(typeof(IEnemy));#
        SpawnEnemyRandomly();

    }

    private void SpawnEnemyRandomly()
    {
        foreach (var point in _spawnPoints)
        {
            var enemy = Instantiate(_enemy, point.position, point.rotation).GetComponent<EnemyBT>();
            enemy._waypoints = _waypoint;

        }

    }
}
