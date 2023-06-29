using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneMovement
{
    private Rigidbody _playerRB;

    [Header("Airborne Modifiers")]
    [Range(0f, 10f)][SerializeField] private float _airborneGravityMultiplier;
    [Range(0f, 10f)][SerializeField] private float _maxAirborneSpeed = 5f;
    [Range(0f, 10f)][SerializeField] private float _airControl = 5f;

    public AirborneMovement(Rigidbody playerRB)
    {
        this._playerRB = playerRB;
    }

    #region Vertical Speed Modifiers

    public Vector3 CalculateMovement(Vector3 moveVal)
    {
        //If no Movement Input, skip setting Movement
        //TODO: Make function to slowly decrease the movementspeed when nothing is pressed
        if (moveVal == Vector3.zero) return _playerRB.velocity;

        var tempMoveVar = moveVal.normalized * Time.deltaTime;
        //Make Movement independent of up and down movement
        tempMoveVar.y = _playerRB.velocity.y;

        Vector3 horizontalVelocity = new Vector3(_playerRB.velocity.x, 0, _playerRB.velocity.z);
        // If air Velocity is higher than max Air Speed, set air Velocity to max Air Speed
        if (horizontalVelocity.magnitude > _maxAirborneSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * _maxAirborneSpeed;
            tempMoveVar.x = horizontalVelocity.x;
            tempMoveVar.z = horizontalVelocity.z;
        }
        // If there is a movement Input set the temp Movement Vector to a value between the player Velocity and the movement Input vlaue * max AirborneSpeed
        else if (moveVal.magnitude > 0)
        {
            Vector3 targetVelocity = moveVal.normalized * _maxAirborneSpeed;
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetVelocity, _airControl * Time.deltaTime);
            tempMoveVar.x = horizontalVelocity.x;
            tempMoveVar.z = horizontalVelocity.z;
        }
        if (_playerRB.velocity.y < 0)
        {
            tempMoveVar.y -= _airborneGravityMultiplier * Time.deltaTime;
        }
        else
        {
            tempMoveVar.y -= _airborneGravityMultiplier / 2 * Time.deltaTime;
        }

        return tempMoveVar;
    }

    #endregion
}
