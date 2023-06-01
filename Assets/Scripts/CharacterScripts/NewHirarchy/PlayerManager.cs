using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : MonoBehaviour//, IDataPersistence
{
    Rigidbody playerRB;
    PlayerInput playerInput;
    Transform mainCamera;
    [HideInInspector] public Vector3 MoveVal;
    private Animator animator;
    private Vector3 positionToSave;
    private Quaternion rotationToSave;

    [Header("Audio Components")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip jumpAudio;

    CameraMovement camMovement;
    GroundMovement horizontalMovement;
    AirborneMovement airborneMovement;
    public GroundCheck groundCheck;
    WallClimbing wallClimbing;

    // public void LoadData(GameData data)
    // {
    //     transform.position = data.playerPosition;
    //     mainCamera.rotation = data.playerCameraRotation;
    // }

    // public void SaveData(ref GameData data)
    // {
    //     data.playerPosition = positionToSave;
    //     data.playerCameraRotation = rotationToSave;
    // }

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        groundCheck = GetComponent<GroundCheck>();//new GroundCheck(this.transform, GetComponent<CapsuleCollider>());
        mainCamera = Camera.main.transform;
        camMovement = new CameraMovement(Camera.main.transform);
        horizontalMovement = new GroundMovement();
        airborneMovement = new AirborneMovement(playerRB);
        playerInput.OnJumpPerformed += GetComponent<IJumpable>().OnJumpPressed;
        wallClimbing = GetComponent<WallClimbing>();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        positionToSave = transform.position;
        rotationToSave = mainCamera.rotation;
    }

    private void FixedUpdate()
    {
        MoveVal = playerInput.MoveVal;
        groundCheck.CheckIfGrounded();

        MoveVal = camMovement.CalculateMoveDirection(MoveVal, playerInput);
        MoveVal = camMovement.FaceMoveDirection(MoveVal, playerRB);

        if (groundCheck.grounded) //Cursor.visible == false)
        {
            //TODO: When Grounded the Movement does not get into the ground fast enough
            playerRB.velocity = horizontalMovement.Move(MoveVal);
            //MovementOptionsCheck();
        }
        else if (wallClimbing != null && wallClimbing.IsClimbing)
        {

            //playerInput.OnJumpPerformed -= GetComponent<JumpingBehaviour>().OnJumpPressed;
            playerRB.velocity = wallClimbing.ClimbingMovement(-MoveVal);
        }
        else
        {
            playerRB.velocity = airborneMovement.CalculateMovement(MoveVal);
        }

    }
}
