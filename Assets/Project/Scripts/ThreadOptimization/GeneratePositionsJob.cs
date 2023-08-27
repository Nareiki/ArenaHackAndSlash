using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct GeneratePositionsJob : IJobParallelFor
{
    public Unity.Mathematics.Random Random;
    public NativeArray<Vector3> Positions;
    public int NumberOfObjects;
    public float InnerRadius;
    public float CheckRadius;
    public float MaxTries;
    public Vector3 ArenaPosition;

    public void Execute(int index)
    {
        Vector3 randomPosition;
        do
        {
            randomPosition = GetRandomPositionWithinArena(InnerRadius);
        }
        while (!CanSpawnAtPosition(randomPosition));

        Positions[index] = randomPosition;
    }


    [BurstCompile]
    private bool CanSpawnAtPosition(Vector3 position)
    {

        // Check if the position is too close to any previously spawned objects.
        foreach (Vector3 spawnedPosition in Positions)
        {
            if (Vector3.Distance(position, spawnedPosition) < CheckRadius)
            {
                return false;
            }
        }

        return true;
    }

    private Vector3 GetRandomPositionWithinArena(float innerRadius)
    {
        // Calculate a random angle and distance within the spawn radius.
        float randomAngle = Random.NextFloat(0, 360);
        float randomDistance = Random.NextFloat(0, innerRadius);

        // Calculate the random position based on polar coordinates.
        float x = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomDistance;
        float z = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomDistance;

        // Add the position of the arena to the random position.
        Vector3 randomPosition = new Vector3(x, 0, z) + ArenaPosition;

        return randomPosition;
    }
}
