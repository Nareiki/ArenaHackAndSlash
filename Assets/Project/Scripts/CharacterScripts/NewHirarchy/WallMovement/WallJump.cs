using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : WallHandlingBase
{
    private PlayerManager _movementScript;

    private int _wallJumpDetect;

    [Header("WallDrag")]
    [Range(0f, 2f)][SerializeField] private float _maxTimeTillDragdown = 1f;

    [Header("Wall Slide Down Speed")]
    [Range(0f, 1f)][SerializeField] private float _slideSpeed;
    private float _dragDownTime;

    [SerializeField] public bool WallJumpEnabled;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _movementScript = GetComponent<PlayerManager>();
        _wallJumpDetect = LayerMask.NameToLayer("Wall");
    }

    /// <summary>
    ///     All Initializing Methods regarding Wall Collisions
    /// </summary>
    /// <param name="collision"></param>
    #region Enter Collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _wallJumpDetect && WallJumpEnabled)
        {
            DefaultInit(collision);
            WallJumpInit();
        }
    }

    private void WallJumpInit()
    {
        if (!_movementScript.GroundCheck.Grounded)
        {
            _dragDownTime = _maxTimeTillDragdown;
        }
    }

    #endregion

    /// <summary>
    ///  All Continuous Calls regarding Walls
    /// </summary>
    /// <param name="collision"></param>
    #region Continuous Collision

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.layer == _wallJumpDetect && WallJumpEnabled)
        {
            Debug.Log("Test wall Jumping");
            WallJumpCheck();
        }
    }

    private void WallJumpCheck()
    {
        if (!_movementScript.GroundCheck.Grounded && _dragDownTime > 0f && _canWallJump)
        {
            _playerRB.velocity = Vector3.zero;
            _playerRB.transform.rotation = Quaternion.Slerp(_playerRB.transform.rotation, Quaternion.LookRotation(_jumpAngle), lookRotationDampFactor * Time.deltaTime);
        }
        else if (!_movementScript.GroundCheck.Grounded && Mathf.Abs(_playerRB.velocity.y) < 0.1f && _canWallJump)
        {
            _dragDownTime = _maxTimeTillDragdown;
        }
        else if (!_movementScript.GroundCheck.Grounded && _playerRB.velocity.y < 0f)
        {
            _playerRB.velocity = new Vector3(_playerRB.velocity.x, _playerRB.velocity.y * _slideSpeed, _playerRB.velocity.z);
        }
        else if (_movementScript.GroundCheck.Grounded)
        {
            _canWallJump = false;
        }
        else
        {
            _canWallJump = true;
        }
        _dragDownTime -= Time.deltaTime;
    }

    #endregion

    /// <summary>
    ///     All Exit Collisions regarding Walls
    /// </summary>
    /// <param name="collision"></param>
    #region Exit Collision

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == _wallJumpDetect && WallJumpEnabled)
        {
            DefaultExit();
            WallJumpExit();
        }
    }

    private void WallJumpExit()
    {
        _dragDownTime = 0f;
    }

    #endregion

    #region Perform Wall Jump
    public override void DoWallJump()
    {
        base.DoWallJump();
    }

    #endregion

}
