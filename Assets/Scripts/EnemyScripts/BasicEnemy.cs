using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    melee,
    ranged,
    mage,
    cavalery
}

public class BasicEnemy : MonoBehaviour, IProduct, IDamageable
{
    public float HealthPoints { get; private set; }

    private string _productName = "BasicEnemy";
    public string ProductName { get => _productName; private set => _productName = value; }

    private void Awake()
    {
        HealthPoints = 100;
    }

    public bool? TakeHit()
    {
        HealthPoints -= 25;
        Debug.Log(HealthPoints);
        bool isDead = HealthPoints <= 0 ? true : false;

        if (isDead) Die();
        return isDead;
    }

    private void Die()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public void Initialize()
    {
        gameObject.name = _productName;
    }
}
