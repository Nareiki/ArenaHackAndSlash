using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WallGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public int wallHeight = 5; // Set the height of the walls.
    public float wallThickness = 0.2f; // Set the thickness of the walls.
    private int numberOfWalls = 16; // Set the number of walls.

    private GameObject[] generatedWalls;

    private int _resolution;  // The actual resolution property.

    // Define a public property that wraps the serialized private field.
    public int Resolution
    {
        get { return _resolution; }
        set
        {
            // Check if the resolution has changed.
            if (value != _resolution)
            {
                _resolution = value;

                // Remove existing walls.
                RemoveWalls();

                // Generate new walls.
                GenerateWalls();
            }
        }
    }

    private float _radius;

    public float Radius
    {
        set
        {
            // Check if the resolution has changed.
            if (value != _radius)
            {
                _radius = value;

                // Remove existing walls.
                RemoveWalls();

                // Generate new walls.
                GenerateWalls();
            }
        }
    }

    private void Start()
    {
        GenerateWalls();
    }

    private void OnValidate()
    {
        RemoveWalls();
        GenerateWalls();
    }



    private void RemoveWalls()
    {
        // Check if there are any generated walls.
        if (generatedWalls != null)
        {
            // Destroy each generated wall.
            foreach (var wall in generatedWalls)
            {

                StartCoroutine(Destroy(wall));

            }

            // Clear the array.
            generatedWalls = null;
        }
    }

    IEnumerator Destroy(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }

    private void GenerateWalls()
    {
        //ProceduralSphere sphere = GetComponentInChildren<ProceduralSphere>();
        //Resolution = sphere.Resolution;
        numberOfWalls = Resolution;
        //_radius = sphere.Radius;

        Vector3 platformCenter = transform.position;
        float angleIncrement = 360f / numberOfWalls;

        // Create an array to store the generated walls.
        generatedWalls = new GameObject[numberOfWalls];

        for (int i = 0; i < numberOfWalls; i++)
        {
            float angle = i * angleIncrement * Mathf.Deg2Rad;
            float nextAngle = (i + 1) * angleIncrement * Mathf.Deg2Rad;

            // Calculate the midpoint between two adjacent vertices.
            float midAngle = (angle + nextAngle) / 2.0f;

            Vector3 vertex1 = new Vector3(Mathf.Cos(angle) * (_radius + wallThickness), 0, Mathf.Sin(angle) * (_radius + wallThickness)) + platformCenter;
            Vector3 vertex2 = new Vector3(Mathf.Cos(nextAngle) * (_radius + wallThickness), 0, Mathf.Sin(nextAngle) * (_radius + wallThickness)) + platformCenter;

            // Calculate the midpoint between the two adjacent vertices.
            Vector3 wallPosition = Vector3.Lerp(vertex1, vertex2, 0.5f);

            Quaternion wallRotation = Quaternion.Euler(0, -midAngle * Mathf.Rad2Deg, 0);

            GameObject wall = Instantiate(wallPrefab, wallPosition, wallRotation);
            BoxCollider collider = wall.AddComponent<BoxCollider>();
            wall.transform.localScale = new Vector3(wallThickness,
                wallHeight, Vector3.Distance(vertex1, vertex2)
                );
            wall.transform.SetParent(transform);
            wall.tag = "Wall";

            generatedWalls[i] = wall;
        }
    }
}
