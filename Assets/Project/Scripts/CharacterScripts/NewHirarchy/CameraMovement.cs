using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement
{
    [Header("Camera Rotation Damp")]
    [Range(1f, 10f)][SerializeField] public float LookRotationDampFactor = 10f;
    public Transform MainCamera;

    public CameraMovement(Transform mainCamera)
    {
        this.MainCamera = mainCamera;
    }

    #region Movement with Camera

    public Vector3 CalculateMoveDirection(Vector3 moveVal, PlayerInput playerInput)
    {
        Vector3 cameraForward = new(MainCamera.forward.x, 0, MainCamera.forward.z);
        Vector3 cameraRight = new(MainCamera.right.x, 0, MainCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * moveVal.y + cameraRight.normalized * moveVal.x;

        moveVal.x = moveDirection.x;
        moveVal.z = moveDirection.z;

        return moveDirection;
    }

    public Vector3 FaceMoveDirection(Vector3 moveVal, Rigidbody playerRB)
    {
        Vector3 faceDirection = new(moveVal.x, 0f, moveVal.z);

        if (faceDirection == Vector3.zero)
            return moveVal;


        playerRB.transform.rotation = Quaternion.Slerp(playerRB.transform.rotation, Quaternion.LookRotation(faceDirection), LookRotationDampFactor * Time.deltaTime);

        return faceDirection;
    }
    #endregion
}
