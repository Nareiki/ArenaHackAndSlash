using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class GroundGenerator : MonoBehaviour
{
    [SerializeField] public float Radius = 10.0f;
    [SerializeField] public int Resolution = 16;

    private Mesh _mesh;

    private int[] _triangles;
    private Vector3[] _verticies;
    private Vector2[] _uv;

    private void Start()
    {
        CreateCircularArena();
        UpdateMesh();
    }

    private void OnValidate()
    {
        CreateCircularArena();
        UpdateMesh();
    }

    private void MeshInitializaion()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        _mesh = meshFilter.mesh;
    }

    private void CreateSphere()
    {
        MeshInitializaion();
        _verticies = new Vector3[(Resolution + 1) * (Resolution + 1)];
        _uv = new Vector2[_verticies.Length];
        _triangles = new int[Resolution * Resolution * 6];

        for (int i = 0; i <= Resolution; i++)
        {
            for (int j = 0; j <= Resolution; j++)
            {
                float u = (float)i / Resolution;
                float v = (float)j / Resolution;
                float theta = u * Mathf.PI * 2;
                float phi = v * Mathf.PI;

                float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = Mathf.Sin(phi) * Mathf.Sin(theta);
                float z = Mathf.Cos(phi);

                _verticies[i * (Resolution + 1) + j] = new Vector3(x, y, z) * Radius;
                _uv[i * (Resolution + 1) + j] = new Vector2(u, v);
            }
        }

        int triangleIndex = 0;
        for (int i = 0; i < Resolution; i++)
        {
            for (int j = 0; j < Resolution; j++)
            {
                int vertIndex = i * (Resolution + 1) + j;
                _triangles[triangleIndex++] = vertIndex;
                _triangles[triangleIndex++] = vertIndex + Resolution + 1;
                _triangles[triangleIndex++] = vertIndex + 1;

                _triangles[triangleIndex++] = vertIndex + 1;
                _triangles[triangleIndex++] = vertIndex + Resolution + 1;
                _triangles[triangleIndex++] = vertIndex + Resolution + 2;
            }
        }
    }

    private void CreateCircularArena()
    {
        MeshInitializaion();

        _verticies = new Vector3[Resolution];
        _uv = new Vector2[Resolution];
        _triangles = new int[(Resolution - 2) * 3]; // Number of triangles needed for a convex polygon.

        float angleIncrement = 360.0f / Resolution; // Angle increment between vertices.

        // Create vertices in a circular pattern.
        for (int i = 0; i < Resolution; i++)
        {
            float angle = i * angleIncrement * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * Radius;
            float z = Mathf.Sin(angle) * Radius;
            _verticies[i] = new Vector3(x, 0, z);
            //_uv[i] = new Vector2((x / (Radius * 2)) + 0.5f, (z / (Radius * 2)) + 0.5f); // Centered UV mapping for texture.
            //_uv[i] = new Vector2(x / Radius, z / Radius); // UV mapping for texture.
            _uv[i] = new Vector2((float)i / Resolution, 0.5f);
        }

        // Create triangles to form the circular shape.
        for (int i = 0, triangleIndex = 0; i < Resolution - 2; i++)
        {
            _triangles[triangleIndex++] = 0;
            _triangles[triangleIndex++] = i + 2;
            _triangles[triangleIndex++] = i + 1;
        }
    }

    private void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _verticies;
        _mesh.uv = _uv;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();

        // Add or update the MeshCollider.
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = _mesh;
        }
    }
}
