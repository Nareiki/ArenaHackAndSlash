using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: aufräumen und umstrukturieren
public class WallClimbing : WallHandlingBase
{
    [Header("Climbing Variables")]
    [Range(50f, 150f)][SerializeField] public float ClimbingSpeed;
    [SerializeField] public bool ClimbingEnabled;
    private LayerMask _climbingWallLayer;
    private bool _canMove = true;

    public override void Start()
    {
        base.Start();
        _climbingWallLayer = LayerMask.NameToLayer("ClimbingWall");
    }

    #region Check for Limitations
    public void OnCollisionEnter(Collision collision)
    {
        if (!(collision.gameObject.layer == _climbingWallLayer && ClimbingEnabled)) return;
        IsClimbing = true;
        ClimbingInit();

        DefaultInit(collision);
    }

    private void ClimbingInit()
    {
        IsClimbing = true;
        _playerRB.useGravity = false;
    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.layer == (int)_climbingWallLayer && ClimbingEnabled)
        {
            Debug.Log("Test Climbing");
            ClimbingCheck();
        }
    }

    private void ClimbingCheck()
    {
        _playerRB.transform.rotation = Quaternion.Slerp(_playerRB.transform.rotation, Quaternion.LookRotation(-_jumpAngle), lookRotationDampFactor * Time.deltaTime);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == _climbingWallLayer && ClimbingEnabled)
        {
            DefaultExit();
            ClimbingExit(collision);
        }
    }

    private void ClimbingExit(Collision collision)
    {
        _playerRB.useGravity = true;
        IsClimbing = false;
        if (_playerRB.velocity.y > 0f)
        {
            //Todo: do the jumping
        }
    }

    #endregion

    public Vector3 ClimbingMovement(Vector3 moveVal)
    {
        if (!_canMove)
        {
            return _playerRB.velocity;
        }
        if (moveVal == Vector3.zero)
        {
            //playerRB.velocity = new Vector3(playerRB.velocity.x, 0, 0);

            return moveVal + transform.forward * 0.1f;
        }

        Vector3 currentAngle = Quaternion.AngleAxis(90, Vector3.up) * _jumpAngle;

        var tempMoveVar = new Vector3(currentAngle.x * moveVal.normalized.z, moveVal.normalized.x, currentAngle.z * moveVal.normalized.z) * ClimbingSpeed * Time.deltaTime;
        return tempMoveVar;
    }

    public override void DoWallJump()
    {
        base.DoWallJump();
        StartCoroutine(DisableMovement());
    }

    IEnumerator DisableMovement()
    {
        _canMove = false;
        yield return new WaitForSeconds(1f);
        _canMove = true;
    }
}
