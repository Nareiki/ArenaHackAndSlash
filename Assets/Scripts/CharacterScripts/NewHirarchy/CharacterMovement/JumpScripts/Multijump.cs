using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multijump : BasicJump
{
    [Header("Multijump Variables")]
    [SerializeField] public bool MultiJumpEnabled = true;
    [SerializeField] public int MaxJumpAmount = 2;
    private int _jumpCount;
    private bool _jumpCoroutineIsRunning;


    public override void OnJumpPressed()
    {
        if (JumpUsed && _jumpCount == MaxJumpAmount)
        {
            _jumpCount--;
        }

        if (!JumpUsed)
        {
            DoJumping();
        }
        else if (MultiJumpHandling())
        {
            DoJumping();
        }
        else
        {
            Debug.Log(_jumpCount);
            Debug.Log("No Jumps Left. '-\\(*_*)/-'");
        }

        //TODO: Implement Input Buffer
    }



    private bool MultiJumpHandling()
    {
        return MultiJumpEnabled && _jumpCount > 0 && _jumpCount < MaxJumpAmount && JumpUsed;
    }

    public override void DoJumping()
    {
        base.DoJumping();
        _jumpCount--;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        _jumpCount = MaxJumpAmount;
    }

    private IEnumerator JumpReset()
    {
        _jumpCoroutineIsRunning = true;
        _jumpCount = MaxJumpAmount;
        yield return new WaitForSeconds(1);
        _jumpCoroutineIsRunning = false;
    }
}
