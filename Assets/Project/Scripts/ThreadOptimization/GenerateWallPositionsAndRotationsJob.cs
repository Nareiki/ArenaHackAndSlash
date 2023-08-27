using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
struct CalculateWallPositionsAndRotationsJob : IJobParallelFor
{
    public int NumberOfWalls;
    public float Radius;
    public float WallThickness;
    public Vector3 PlatformCenter;

    [NativeDisableParallelForRestriction]
    public NativeArray<Vector3> WallPositions;

    [NativeDisableParallelForRestriction]
    public NativeArray<Quaternion> WallRotations;

    [NativeDisableParallelForRestriction]
    public NativeArray<VertexData> Vertexes;

    public void Execute(int index)
    {
        float angleIncrement = 360f / NumberOfWalls;
        float angle = index * angleIncrement * Mathf.Deg2Rad;
        float nextAngle = (index + 1) * angleIncrement * Mathf.Deg2Rad;

        // Calculate the midpoint between two adjacent vertices.
        float midAngle = (angle + nextAngle) / 2.0f;

        Vector3 vertex1 = new Vector3(Mathf.Cos(angle) * (Radius + WallThickness), 0, Mathf.Sin(angle) * (Radius + WallThickness)) + PlatformCenter;
        Vector3 vertex2 = new Vector3(Mathf.Cos(nextAngle) * (Radius + WallThickness), 0, Mathf.Sin(nextAngle) * (Radius + WallThickness)) + PlatformCenter;

        VertexData vertexData = new VertexData(vertex1, vertex2);
        Vertexes[index] = vertexData;

        // Calculate the midpoint between the two adjacent vertices.
        Vector3 wallPosition = Vector3.Lerp(vertex1, vertex2, 0.5f);

        Quaternion wallRotation = Quaternion.Euler(0, -midAngle * Mathf.Rad2Deg, 0);

        WallPositions[index] = wallPosition;
        WallRotations[index] = wallRotation;
    }
}
