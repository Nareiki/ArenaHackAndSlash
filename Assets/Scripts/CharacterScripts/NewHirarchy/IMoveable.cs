using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    int _runSpeed { get; }

    public Vector3 CalculateMovement();
}
