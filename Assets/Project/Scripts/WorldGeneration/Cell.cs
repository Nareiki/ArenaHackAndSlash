using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private Vector3 _position;
    //public CellTag zone;
    //public CellSideTag side;

    public Cell(Vector3 position)
    {
        _position = position;
    }

    public override string ToString()
    {
        return _position.ToString();
    }
}
