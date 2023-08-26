using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaGenerator : MonoBehaviour
{
    [SerializeField] private int seed;
    [SerializeField] private Vector2 _roomSize;
    [SerializeField] private Vector2 _wallSize;

    [SerializeField] private Mesh _wallMesh;
    [SerializeField] private Mesh _wallMeshB;
    [SerializeField] private Mesh _wallMeshC;

    [SerializeField] private Mesh _pillarMesh;
    [SerializeField] private Mesh _floorMesh;

    [SerializeField] private Material _wallMaterial0;
    [SerializeField] private Material _wallMaterial1;
    private List<Matrix4x4> _wallMatricesN;
    private List<Matrix4x4> _wallMatricesNB;
    private List<Matrix4x4> _wallMatricesNC;


    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private int _gridOffset = 2;
    [SerializeField] private GameObject _blockGameObject;
    [SerializeField] private float _noiseHeight = 3;

    [SerializeField] GameObject player;

    private Vector3 _startPosition = Vector3.zero;
    private Hashtable blockContainer = new Hashtable();

    private List<Vector3> blockPosition = new List<Vector3>();

    public int xPlayerMove
    {
        get
        {
            return (int)(player.transform.position.x - _startPosition.x);
        }
    }

    public int zPlayerMove
    {
        get
        {
            return (int)(player.transform.position.z - _startPosition.z);
        }
    }

    public int xPlayerLocation
    {
        get
        {
            return (int)Mathf.Floor(player.transform.position.x);
        }
    }

    public int zPlayerLocation
    {
        get
        {
            return (int)Mathf.Floor(player.transform.position.z);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //CreateWalls();
        //RenderWall();

        for (int x = (int)-_roomSize.x; x < _roomSize.x; x++)
        {
            for (int z = (int)-_roomSize.y; z < _roomSize.y; z++)
            {
                Vector3 pos = new Vector3(x * _gridOffset + _startPosition.x,
                    GenerateNoise(x, z, 8f) * _noiseHeight,
                    z * _gridOffset + _startPosition.z);


                GameObject block = Instantiate(_blockGameObject, pos, Quaternion.identity) as GameObject;


                blockContainer.Add(pos, block);

                blockPosition.Add(block.transform.position);

                block.transform.SetParent(this.transform);
            }
        }
        SpawnObject();
    }

    void Update()
    {
        if (Mathf.Abs(xPlayerMove) > 1 || Mathf.Abs(zPlayerMove) >= 1)
        {
            for (int x = (int)-_roomSize.x; x < _roomSize.x; x++)
            {
                for (int z = (int)-_roomSize.y; z < _roomSize.y; z++)
                {
                    Vector3 pos = new Vector3(x * _gridOffset + xPlayerLocation,
                        GenerateNoise(x + xPlayerLocation, z + zPlayerLocation, 8f) * _noiseHeight,
                        z * _gridOffset + zPlayerLocation);

                    if (!blockContainer.ContainsKey(pos))
                    {
                        GameObject block = Instantiate(_blockGameObject, pos, Quaternion.identity) as GameObject;


                        blockContainer.Add(pos, block);

                        blockPosition.Add(block.transform.position);

                        block.transform.SetParent(this.transform);
                    }
                }
            }
        }

        //CreateWalls();
        //RenderWall();

        //if (AnyChanges())
        //{
        //    CreateWalls();
        //    CreatePillars();
        //    CreateFloor();
        //}

        //RenderWalls();
        //RenderPillars();
        //RenderFloor();
    }

    private void SpawnObject()
    {
        for (int c = 0; c < 20; c++)
        {
            GameObject toPlaceObject = Instantiate(_objectToSpawn,
                ObjectSpawnLocation(),
                Quaternion.identity);
        }
    }

    private Vector3 ObjectSpawnLocation()
    {
        int rdnIndex = Random.Range(0, blockPosition.Count);
        Vector3 newPos = new Vector3(
            blockPosition[rdnIndex].x,
            blockPosition[rdnIndex].y + 0.5f,
            blockPosition[rdnIndex].z
        );

        blockPosition.RemoveAt(rdnIndex);
        return newPos;
    }

    private float GenerateNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + transform.position.x) / detailScale;
        float zNoise = (z + transform.position.x) / detailScale;

        return Mathf.PerlinNoise(xNoise, zNoise);
    }

    private bool AnyChanges()
    {
        throw new System.NotImplementedException();
    }

    private void RenderFloor()
    {
        throw new System.NotImplementedException();
    }

    private void RenderPillars()
    {
        throw new System.NotImplementedException();
    }

    private void RenderWalls()
    {
        throw new System.NotImplementedException();
    }

    private void CreateFloor()
    {
        throw new System.NotImplementedException();
    }

    private void CreatePillars()
    {
        throw new System.NotImplementedException();
    }

    private void CreateWalls()
    {
        Random.InitState(seed);
        _wallMatricesN = new List<Matrix4x4>();
        _wallMatricesNB = new List<Matrix4x4>();
        _wallMatricesNC = new List<Matrix4x4>();

        int wallCount = Mathf.Max(1, (int)(_roomSize.x / _wallSize.x));
        float scale = (_roomSize.x / wallCount) / _wallSize.x;

        for (int i = 0; i < wallCount; i++)
        {
            // Circular Wall
            //var pos = (new Vector3(-r * Mathf.Cos(angle), 0, r * Mathf.Sin(angle)));
            var t = transform.position + new Vector3(-_roomSize.x / 2 + _wallSize.x * scale / 2 + i * scale * _wallSize.x, 0, +_roomSize.y / 2);
            var r = transform.rotation;
            var s = new Vector3(scale, 1, 1);

            //var position = transform.position + new Vector3(-_roomSize.x / 2 + _wallSize.x * scale / 2 + i * scale * _wallSize.x, 0, -_roomSize.y / 2);

            var mat = Matrix4x4.TRS(t, r, s);

            var rand = Random.Range(0, 3);
            if (rand < 1)
            {
                _wallMatricesN.Add(mat);
            }
            else if (rand < 2)
            {
                _wallMatricesNB.Add(mat);
            }
            else
            {
                _wallMatricesNC.Add(mat);
            }
            //_wallMatricesN.Add(mat);

        }
    }

    private void RenderWall()
    {
        if (_wallMatricesN != null)
        {
            Graphics.DrawMeshInstanced(_wallMesh, 0, _wallMaterial0, _wallMatricesN.ToArray(), _wallMatricesN.Count);
            Graphics.DrawMeshInstanced(_wallMesh, 1, _wallMaterial1, _wallMatricesN.ToArray(), _wallMatricesN.Count);
        }

        if (_wallMatricesNB != null)
        {
            Graphics.DrawMeshInstanced(_wallMeshB, 0, _wallMaterial0, _wallMatricesNB.ToArray(), _wallMatricesNB.Count);
            Graphics.DrawMeshInstanced(_wallMeshB, 1, _wallMaterial1, _wallMatricesNB.ToArray(), _wallMatricesNB.Count);
        }

        if (_wallMatricesNC != null)
        {
            Graphics.DrawMeshInstanced(_wallMeshC, 0, _wallMaterial0, _wallMatricesNC.ToArray(), _wallMatricesNC.Count);
            Graphics.DrawMeshInstanced(_wallMeshC, 1, _wallMaterial1, _wallMatricesNC.ToArray(), _wallMatricesNC.Count);
        }
    }
}
