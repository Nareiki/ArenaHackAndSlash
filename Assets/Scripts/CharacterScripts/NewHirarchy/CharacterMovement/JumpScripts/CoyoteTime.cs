using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IJumpable))]
public class CoyoteTime : MonoBehaviour, ICoyoteCheckable
{
    public IJumpable JumpScript { get; private set; }

    [Header("Coyote Time adjustment")]
    [SerializeField] private float _coyoteTime = 0.5f;
    private float _coyoteTimeCounter;

    //private bool inputBufferRestart;
    //private float lastJumpPressed;

    private void Start()
    {
        JumpScript = GetComponent<IJumpable>();
    }

    private void Update()
    {
        CoyoteTimeTimer();
    }

    /// <summary>
    ///     Checks the State of the Coyote Time
    /// </summary>
    /// <returns>If Coyote Time Applies return true else false</returns>
    public bool StateCheck()
    {
        return CoyoteTimeCheck();
    }

    public bool CoyoteTimeCheck()
    {
        //lastJumpPressed = Time.time;

        if (_coyoteTimeCounter <= 0f)
        {
            return false;
        }
        else if (_coyoteTimeCounter > 0f)
        {
            return true;
        }
        return false;

        //TODO: Implement Input Buffer
    }

    private void CoyoteTimeTimer()
    {
        if (!JumpScript.JumpUsed)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else if (JumpScript.JumpUsed)
        {
            _coyoteTimeCounter = 0;
        }

        _coyoteTimeCounter -= Time.deltaTime;
    }
}
