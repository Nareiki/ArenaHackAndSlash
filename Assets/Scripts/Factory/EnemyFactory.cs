using UnityEngine;
using System.Collections;

public class EnemyFactory : Factory
{
    [SerializeField] private BasicEnemy _basicEnemy;


    public override IProduct GetEnemy(Vector3 position)
    {
        // create a Prefab instance and get the product component
        GameObject inst = Instantiate(_basicEnemy.gameObject, position, Quaternion.identity);
        BasicEnemy enemy = inst.GetComponent<BasicEnemy>();

        // each product contains its own logic
        enemy.Initialize();

        return enemy;
    }
}

