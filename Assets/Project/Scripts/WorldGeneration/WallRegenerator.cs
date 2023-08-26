using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(WallGenerator))]
[RequireComponent(typeof(GroundGenerator))]
public class WallRegenerator : MonoBehaviour
{
    private WallGenerator _wallGenerator; // Reference to the WallGenerator script.
    private GroundGenerator _proceduralSphere;

    private int _previousResolution; // Store the previous resolution value.
    private float _previousRadius;  // Store the previous radius value

    private void Awake()
    {
        _wallGenerator = GetComponent<WallGenerator>();
        _proceduralSphere = GetComponent<GroundGenerator>();
    }

    private void Start()
    {
        // Ensure that you have assigned the WallGenerator script reference in the Inspector.
        if (_wallGenerator != null)
        {
            // Initialize the previousResolution with the initial value.
            _wallGenerator.Resolution = _proceduralSphere.Resolution;
            _wallGenerator.Radius = _proceduralSphere.Radius;
        }
        else
        {
            Debug.LogWarning("WallGenerator reference not assigned in AnotherClass.");
        }
    }

    private void Update()
    {
        // Check if the WallGenerator reference is not null.
        if (_wallGenerator != null && _proceduralSphere != null)
        {
            if (_proceduralSphere.Resolution != _previousResolution)
            {
                _wallGenerator.Resolution = _proceduralSphere.Resolution;
                _previousResolution = _wallGenerator.Resolution;

            }

            if (_proceduralSphere.Radius != _previousRadius)
            {
                _wallGenerator.Radius = _proceduralSphere.Radius;
                _previousRadius = _proceduralSphere.Radius;
            }
        }
    }
}
