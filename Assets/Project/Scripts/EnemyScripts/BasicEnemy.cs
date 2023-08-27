using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] private GameObject _textPrefab;

    private void Awake()
    {
        HealthPoints = 100;
    }
    private void OnEnable()
    {
        HealthPoints = 100;
    }

    public bool TakeHit(float damage)
    {
        HealthPoints -= damage;

        ShowFloatingText("-" + damage.ToString());

        Debug.Log(HealthPoints);
        bool isDead = HealthPoints <= 0 ? true : false;

        if (isDead) Die();
        return isDead;
    }

    private void ShowFloatingText(string text)
    {
        // Instantiate the floating text prefab.
        GameObject floatingText = Instantiate(_textPrefab, transform.position, Quaternion.identity);

        // Set the text content.
        FloatingText floatingTextScript = floatingText.GetComponent<FloatingText>();
        if (floatingTextScript != null)
        {
            floatingTextScript.SetText(text);
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
        MSEnemySpawn.Publish(this);
    }

    public void Initialize()
    {
        gameObject.name = _productName;
        HealthPoints = 100;
    }
}
