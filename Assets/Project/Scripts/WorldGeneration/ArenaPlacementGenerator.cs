using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPlacementGenerator : MonoBehaviour
{
    public GameObject towerPrefab;
    public int numTowers = 10; // Number of towers to place.

    void Start()
    {
        GenerateArena();
        PlaceTowers();
    }

    void GenerateArena()
    {
        // No need for generating a sphere; we'll use the ProceduralSphere script.
    }

    void PlaceTowers()
    {
        GroundGenerator sphere = GetComponentInChildren<GroundGenerator>();

        for (int i = 0; i < numTowers; i++)
        {
            // Randomly select a point on the sphere's surface.
            Vector3 randomPoint = sphere.transform.position + Random.onUnitSphere * sphere.Radius;

            // Instantiate a tower at the selected point.
            GameObject tower = Instantiate(towerPrefab, randomPoint, Quaternion.identity);

            // Make the tower a child of the arena for organization.
            tower.transform.parent = transform;
        }
    }
}
