using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int _enemyHealthPoints = 100;

    private void Awake()
    {
        _enemyHealthPoints = 100;
    }

    public bool TakeHit()
    {
        _enemyHealthPoints -= 25;
        Debug.Log(_enemyHealthPoints);
        bool isDead = _enemyHealthPoints <= 0 ? true : false;

        if (isDead) Die();
        return isDead;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
