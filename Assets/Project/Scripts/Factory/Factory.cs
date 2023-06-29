using System;
using UnityEngine;

public interface IProduct
{
    public string ProductName { get; }

    public void Initialize();


}

public abstract class Factory : MonoBehaviour
{
    public abstract IProduct GetEnemy(Vector3 position);

}

