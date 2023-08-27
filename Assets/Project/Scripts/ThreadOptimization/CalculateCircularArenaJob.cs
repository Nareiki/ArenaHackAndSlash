using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
struct CalculateCircularArenaJob : IJobParallelFor
{
    public int Resolution;
    public float Radius;
    [NativeDisableParallelForRestriction] public NativeArray<Vector3> Vertices;
    [NativeDisableParallelForRestriction] public NativeArray<Vector2> UV;
    [NativeDisableParallelForRestriction] public NativeArray<int> Triangles;

    public void Execute(int index)
    {
        float angleIncrement = 360.0f / Resolution;
        float angle = index * angleIncrement * Mathf.Deg2Rad;
        float x = Mathf.Cos(angle) * Radius;
        float z = Mathf.Sin(angle) * Radius;
        Vertices[index] = new Vector3(x, 0, z);
        UV[index] = new Vector2((float)index / Resolution, 0.5f);

        if (index < Resolution - 2)
        {
            Triangles[index * 3] = 0;
            Triangles[index * 3 + 1] = index + 2;
            Triangles[index * 3 + 2] = index + 1;
        }
    }
}
