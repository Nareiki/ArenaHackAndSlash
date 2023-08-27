using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class WallGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private int _wallHeight = 5;
    [SerializeField] private float _wallThickness = 0.2f; 
    private int _numberOfWalls = 16; // Number of walls is dependent on the resolution of the Arena Floor.

    private GameObject[] _generatedWalls;

    private int _resolution;

    public int Resolution
    {
        get { return _resolution; }
        set
        {
            if (value != _resolution)
            {
                _resolution = value;

                RemoveWalls();
                GenerateWalls();
            }
        }
    }

    private float _radius;

    public float Radius
    {
        set
        {
            if (value != _radius)
            {
                _radius = value;
                RemoveWalls();
                GenerateWalls();
            }
        }
    }

    private void Start()
    {
        RemoveWalls();
        GenerateWalls();
    }

    private void OnValidate()
    {
        RemoveWalls();
        GenerateWalls();
    }

    private void RemoveWalls()
    {
        if (_generatedWalls != null)
        {
            foreach (var wall in _generatedWalls)
            {
                StartCoroutine(Destroy(wall));
            }
            _generatedWalls = null;
        }
    }

    IEnumerator Destroy(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }

    private void GenerateWalls()
    {
        _numberOfWalls = Resolution;

        Vector3 platformCenter = transform.position;
        float angleIncrement = 360f / _numberOfWalls;

        // Create an array to store the generated walls.
        _generatedWalls = new GameObject[_numberOfWalls];

        NativeArray<Vector3> wallPositions = new NativeArray<Vector3>(_numberOfWalls, Allocator.TempJob);
        NativeArray<Quaternion> wallRotations = new NativeArray<Quaternion>(_numberOfWalls, Allocator.TempJob);
        NativeArray<VertexData> vertexes = new NativeArray<VertexData>(_numberOfWalls, Allocator.TempJob);

        CalculateWallPositionsAndRotationsJob job = new CalculateWallPositionsAndRotationsJob
        {
            NumberOfWalls = _numberOfWalls,
            Radius = _radius,
            WallThickness = _wallThickness,
            PlatformCenter = platformCenter,
            WallPositions = wallPositions,
            WallRotations = wallRotations,
            Vertexes = vertexes,
        };

        JobHandle jobHandle = job.Schedule(_numberOfWalls, 64);

        jobHandle.Complete();
        
        for(int i = 0; i< _numberOfWalls; i++)
        {
            GameObject wall = SpawnWall(wallPositions[i], wallRotations[i], vertexes[i]);

            _generatedWalls[i] = wall;
        }

        wallPositions.Dispose();
        wallRotations.Dispose();
        vertexes.Dispose();

        #region non Job System
        //for (int i = 0; i < _numberOfWalls; i++)
        //{
        //    float angle = i * angleIncrement * Mathf.Deg2Rad;
        //    float nextAngle = (i + 1) * angleIncrement * Mathf.Deg2Rad;

        //    // Calculate the midpoint between two adjacent vertices.
        //    float midAngle = (angle + nextAngle) / 2.0f;

        //    Vector3 vertex1 = new Vector3(Mathf.Cos(angle) * (_radius + _wallThickness), 0, Mathf.Sin(angle) * (_radius + _wallThickness)) + platformCenter;
        //    Vector3 vertex2 = new Vector3(Mathf.Cos(nextAngle) * (_radius + _wallThickness), 0, Mathf.Sin(nextAngle) * (_radius + _wallThickness)) + platformCenter;

        //    // Calculate the midpoint between the two adjacent vertices.
        //    Vector3 wallPosition = Vector3.Lerp(vertex1, vertex2, 0.5f);

        //    Quaternion wallRotation = Quaternion.Euler(0, -midAngle * Mathf.Rad2Deg, 0);

        //    GameObject wall = Instantiate(_wallPrefab, wallPosition, wallRotation);
        //    BoxCollider collider = wall.AddComponent<BoxCollider>();
        //    wall.transform.localScale = new Vector3(_wallThickness,
        //        _wallHeight, Vector3.Distance(vertex1, vertex2)
        //        );
        //    wall.transform.SetParent(transform);
        //    wall.tag = "Wall";

        //    _generatedWalls[i] = wall;
        //}
        #endregion
    }

    private GameObject SpawnWall(Vector3 position, Quaternion rotation, VertexData vertexes)
    {
        GameObject wall = Instantiate(_wallPrefab, position, rotation);
        BoxCollider collider = wall.AddComponent<BoxCollider>();
        wall.transform.localScale = new Vector3(_wallThickness,
        _wallHeight, Vector3.Distance(vertexes.Vertex1, vertexes.Vertex2)
            );
        wall.transform.SetParent(transform);
        wall.tag = "Wall";

        return wall;
    }
}

public struct VertexData
{
    public Vector3 Vertex1;
    public Vector3 Vertex2;

    public VertexData(Vector3 vertex1, Vector3 vertex2)
    {
        Vertex1 = vertex1;
        Vertex2 = vertex2;
    }
}
