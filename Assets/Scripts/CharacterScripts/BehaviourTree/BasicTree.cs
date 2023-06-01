using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicTree : MonoBehaviour
{

    private BasicNode _root = null;

    void Start()
    {
        _root = SetupTree();
    }

    void Update()
    {
        if (_root != null)
            _root.Evaluate();
    }

    protected abstract BasicNode SetupTree();
}
