using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using static UnityEditor.Progress;

[ExecuteInEditMode]
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab; // The prefab of the object you want to spawn.
    [SerializeField] private int numberOfObjects = 10; // Number of objects to spawn.
    [SerializeField] private float _minDistanceFromEdge = 2.0f;
    [SerializeField] private float _checkRadius = 1.0f;
    [SerializeField] private float _maxTries = 10;
    NativeArray<Unity.Mathematics.Random> _randomNumberGenerators;

    private float _arenaRadius = 5.0f; // Radius within which objects will be spawned.

    private GameObject[] _gameObjects;

    private List<Vector3> _spawnedObjectPositions = new List<Vector3>();

    void Awake()
    {
        _randomNumberGenerators = new NativeArray<Unity.Mathematics.Random>(Unity.Jobs.LowLevel.Unsafe.JobsUtility.MaxJobThreadCount, Allocator.Persistent);

        for (int i = 0; i < _randomNumberGenerators.Length; i++)
        {
            _randomNumberGenerators[i] = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, int.MaxValue));
        }
    }

    public float ArenaRadius
    {
        get { return _arenaRadius; }
        set {
            if (value != _arenaRadius)
            {
                _arenaRadius = value;

                RemoveObjects();
                SpawnObjects();
            }
        }
    }

    private void Start()
    {
        RemoveObjects();
        SpawnObjects();
    }

    private void RemoveObjects()
    {
        // Check if there are any generated walls.
        if (_gameObjects != null)
        {
            // Destroy each generated wall.
            foreach (var wall in _gameObjects)
            {

                StartCoroutine(Destroy(wall));

            }

            // Clear the array.
            _spawnedObjectPositions.Clear();
            _gameObjects = null;
        }
    }

    IEnumerator Destroy(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }

    private void SpawnObjects()
    {
        _gameObjects = new GameObject[numberOfObjects];
        float innerRadius = ArenaRadius - _minDistanceFromEdge;

        NativeArray<Vector3> positions = new NativeArray<Vector3>(numberOfObjects, Allocator.Persistent);
        

        GeneratePositionsJob job = new GeneratePositionsJob
        {
            RandomNumberGenerators = _randomNumberGenerators,
            Positions = positions,
            NumberOfObjects = numberOfObjects,
            InnerRadius = innerRadius,
            CheckRadius = _checkRadius,
            MaxTries = _maxTries,
            ArenaPosition = transform.position,
        };

        JobHandle jobHandle = job.Schedule(numberOfObjects, 64);

        jobHandle.Complete();

        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 spawnPosition = positions[i];

            if(spawnPosition != Vector3.zero)
            {
                _spawnedObjectPositions.Add(spawnPosition);
                _gameObjects[i] = SpawnObject(spawnPosition);
            }

            //GameObject spawnedObject = TrySpawnObject(innerRadius);

            //int tryCounter = 0;

            //while (spawnedObject == null)
            //{
            //    spawnedObject = TrySpawnObject(innerRadius);
            //    tryCounter++;
            //}

            //_gameObjects[i] = spawnedObject;
            
        }
        positions.Dispose();
    }

    private GameObject TrySpawnObject(float innerRadius)
    {
        Vector3 randomPosition = GetRandomPositionWithinArena(innerRadius);

        for (int i = 0;i<_maxTries; i++)
        {
            if (CanSpawnAtPosition(randomPosition))
            {
                break;
            }
            randomPosition = GetRandomPositionWithinArena(innerRadius);
        }

        _spawnedObjectPositions.Add(randomPosition);

        return SpawnObject(randomPosition);
    }

    private GameObject SpawnObject(Vector3 spawnPosition)
    {
        GameObject item = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
        BoxCollider collider = item.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        return item;
    }

    private bool CanSpawnAtPosition(Vector3 position)
    {
        // Check if the position is too close to any previously spawned objects.
        foreach (Vector3 spawnedPosition in _spawnedObjectPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < _checkRadius)
            {
                return false;
            }
        }

        return true;
    }

    private Vector3 GetRandomPositionWithinArena(float innerRadius)
    {
        // Calculate a random angle and distance within the spawn radius.
        float randomAngle = Random.Range(0, 360);
        float randomDistance = Random.Range(0, innerRadius);

        // Calculate the random position based on polar coordinates.
        float x = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomDistance;
        float z = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomDistance;

        // Add the position of the arena to the random position.
        Vector3 randomPosition = new Vector3(x, 0, z) + transform.position;

        return randomPosition;
    }
}
