using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyType
{
    melee,
    ranged,
    mage,
    cavalery
}

public class BasicEnemy : MonoBehaviour, IEnemy
{
    public float HealthPoints { get; private set; }


    private void Awake()
    {
        HealthPoints = 100;
    }

    public bool TakeHit()
    {
        HealthPoints -= 25;
        Debug.Log(HealthPoints);
        bool isDead = HealthPoints <= 0 ? true : false;

        if (isDead) Die();
        return isDead;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
