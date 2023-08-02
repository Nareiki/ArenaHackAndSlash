using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGraph : MonoBehaviour
{
    static readonly int positionsId = Shader.PropertyToID("positionsId");
    static readonly int resolutionId = Shader.PropertyToID("resolutionId");
    static readonly int stepId = Shader.PropertyToID("stepId");
    static readonly int timeId = Shader.PropertyToID("timeId");

    [SerializeField] private ComputeShader mGraphShader;
    [SerializeField] private Material mPointMat;
    [SerializeField] private Mesh mPointMesh;

    private const int maxResolution = 1000;

    [SerializeField, Range(2, maxResolution)] private int mResolution;

    ComputeBuffer positionsBuffer;

    private void Awake()
    {
        positionsBuffer = new ComputeBuffer(mResolution * mResolution, 3 * sizeof(float));
    }

    private void Update()
    {
        UpdateFunction();
    }

    private void UpdateFunction()
    {
        float step = 2f / mResolution;

        mGraphShader.SetInt(resolutionId, mResolution);
        mGraphShader.SetFloat(stepId, step);
        mGraphShader.SetFloat(timeId, Time.time);

        //ComputeBuffer positionsBuffer = new ComputeBuffer(mResolution * mResolution, 3 * sizeof(float));
        mGraphShader.SetBuffer(0, positionsId, positionsBuffer);

        // von [numthreads(8,8,1)] im Compute Shader
        int groupCount = Mathf.CeilToInt(mResolution / 8f);
        mGraphShader.Dispatch(0, groupCount, groupCount, 1);

        mPointMat.SetBuffer(positionsId, positionsBuffer);
        mPointMat.SetFloat(stepId, step);

        // viereck in dem der Prozess reinzeichnen darf
        Bounds bounds = new Bounds(Vector3.zero, Vector3.one * (2f +2f / mResolution));
        Graphics.DrawMeshInstancedProcedural(mPointMesh, 0, mPointMat, bounds, mResolution * mResolution);
    }

    private void OnDestroy()
    {
        positionsBuffer.Release();
    }
}
