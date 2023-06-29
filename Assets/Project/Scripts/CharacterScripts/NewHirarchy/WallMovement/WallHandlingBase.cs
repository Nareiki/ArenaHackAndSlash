using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHandlingBase : MonoBehaviour
{
    public Rigidbody _playerRB;
    internal PlayerInput _playerInput;

    internal Vector3 _jumpAngle;

    internal float _jumpforce = 5;
    internal bool _canWallJump;
    internal float lookRotationDampFactor = 1;

    [HideInInspector] public bool IsClimbing;


    // Start is called before the first frame update
    public virtual void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
    }

    internal void DefaultInit(Collision collision)
    {
        _playerInput.OnJumpPerformed += DoWallJump;
        _jumpAngle = collision.contacts[0].normal;
        _playerRB.velocity = Vector3.zero;
        _canWallJump = true;
    }

    internal void DefaultExit()
    {
        _playerInput.OnJumpPerformed -= DoWallJump;
    }

    #region Perform Wall Jump
    public virtual void DoWallJump()
    {
        if (!IsClimbing) return;
        _playerRB.velocity = new Vector3(_playerRB.velocity.x, 0, _playerRB.velocity.z);
        _playerRB.AddForce((_jumpAngle + Vector3.up) * _jumpforce, ForceMode.Impulse);
        Debug.Log("Jump");
        //StartCoroutine(DisableMovement());
    }
    #endregion
}
