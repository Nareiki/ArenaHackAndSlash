using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement
{

    [Header("Player Speed")]
    //[Range(100f, 200f)][SerializeField] float movementSpeed = 150f;
    [Range(200f, 350f)][SerializeField] private float _runSpeed = 250f;

    [Header("Ground Gravity")]
    [Range(0f, 10f)][SerializeField] private float _groundGravityMultiplier;


    public Vector3 Move(Vector3 moveVal)
    {
        //Temporar Movement Variable to use, apply at the end of the method
        var tempMoveVar = moveVal.normalized * _runSpeed * Time.deltaTime;
        //Make Movement independent of up and down movement
        //tempMoveVar.y = playerRB.velocity.y;


        tempMoveVar.y -= _groundGravityMultiplier * Time.deltaTime;

        return tempMoveVar;
    }
}
