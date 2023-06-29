using UnityEngine;
using System.Collections;

public interface IFactory
{
    public GameObject ObjectToSpawn { get; }

    public IProduct SpawnObject(Vector3 position);

}

