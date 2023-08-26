using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private int _worldx;
    [SerializeField] private int _worldz;

    private Mesh _mesh;

    private int[] _triangles;
    private Vector3[] _verticies;

    // Start is called before the first frame update
    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        GenerateMesh();
        UpdateMesh();
    }

    private void GenerateMesh()
    {
        _triangles = new int[_worldx * _worldz * 6];
        _verticies = new Vector3[((_worldx + 1) * (_worldz + 1))];

        for (int i = 0, z = 0; z <= _worldz; z++)
        {
            for (int x = 0; x <= _worldx; x++)
            {
                _verticies[i] = new Vector3(x, 0, z);
                i++;
            }
        }

        int tris = 0;
        int verts = 0;

        for (int z = 0; z < _worldz; z++)
        {
            for (int x = 0; x < _worldx; x++)
            {
                _triangles[tris + 0] = verts + 0;
                _triangles[tris + 1] = verts + _worldz + 1;
                _triangles[tris + 2] = verts + 1;

                _triangles[tris + 3] = verts + 1;
                _triangles[tris + 4] = verts + _worldz + 1;
                _triangles[tris + 5] = verts + _worldz + 2;

                verts++;

                tris += 6;
            }
            verts++;
        }
    }

    private void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _verticies;
        _mesh.triangles = _triangles;

        _mesh.RecalculateNormals();
    }
}
