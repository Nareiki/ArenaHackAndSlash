using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class BasicTree : MonoBehaviour
{

    private BasicNode _root = null;

    //private float _timer = 0f;
    //private float _timerCounter = 0.5f;

    void Start()
    {
        _root = SetupTree();
        //StartCoroutine(Update1());
    }

    private void Update()
    {
        if (_root != null)
            _root.Evaluate();

    }

    //IEnumerator Update1()
    //{
    //    //_timer += Time.deltaTime;

    //    //if (_timer > _timerCounter)
    //    //{
    //    //_timer = 0;
    //    while (true)
    //    {

    //        if (_root != null)
    //            _root.Evaluate();

    //        yield return new WaitForSeconds(0.5f);

    //    }

    //    //}

    //}

    protected abstract BasicNode SetupTree();
}
