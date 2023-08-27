using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(WallGenerator))]
[RequireComponent(typeof(GroundGenerator))]
[RequireComponent(typeof(ObjectSpawner))]
public class WorldObjectManager : MonoBehaviour
{
    private WallGenerator _wallGenerator; // Reference to the WallGenerator script.
    private GroundGenerator _proceduralSphere;
    private ObjectSpawner _spawner;

    private int _previousResolution; // Store the previous resolution value.
    private float _previousRadius;  // Store the previous radius value

    private void Awake()
    {
        _wallGenerator = GetComponent<WallGenerator>();
        _proceduralSphere = GetComponent<GroundGenerator>();
        _spawner = GetComponent<ObjectSpawner>();
    }

    private void Start()
    {
        if (_wallGenerator != null)
        {
            _wallGenerator.Resolution = _proceduralSphere.Resolution;
            _wallGenerator.Radius = _proceduralSphere.Radius;
        }
        else
        {
            Debug.LogWarning("WallGenerator reference not assigned in AnotherClass.");
        }

        if(_spawner != null)
        {
            _spawner.ArenaRadius = _proceduralSphere.Radius;
        }
    }
    // Connection between the Generation Scripts
    private void Update()
    {
        if (_wallGenerator != null && _proceduralSphere != null)
        {
            int resolution = _proceduralSphere.Resolution;

            if (resolution != _previousResolution)
            {
                _wallGenerator.Resolution = resolution;
                _previousResolution = _wallGenerator.Resolution;
            }

            float radius = _proceduralSphere.Radius;

            if (radius != _previousRadius)
            {
                _wallGenerator.Radius = radius;
                _previousRadius = radius;
                _spawner.ArenaRadius = radius;
            }
        }
    }
}