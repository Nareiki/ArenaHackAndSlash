using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : MonoBehaviour//, IDataPersistence
{
    private Rigidbody _playerRB;
    private PlayerInput _playerInput;
    private Transform _mainCamera;
    [HideInInspector] public Vector3 MoveVal;
    private Animator _animator;
    private Vector3 _positionToSave;
    private Quaternion _rotationToSave;

    [Header("Audio Components")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _jumpAudio;

    private CameraMovement _camMovement;
    private GroundMovement _horizontalMovement;
    private AirborneMovement _airborneMovement;
    public GroundCheck GroundCheck;
    private WallClimbing _wallClimbing;

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
        _playerRB = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponentInChildren<Animator>();
        GroundCheck = GetComponent<GroundCheck>();//new GroundCheck(this.transform, GetComponent<CapsuleCollider>());
        _mainCamera = Camera.main.transform;
        _camMovement = new CameraMovement(Camera.main.transform);
        _horizontalMovement = new GroundMovement();
        _airborneMovement = new AirborneMovement(_playerRB);
        _playerInput.OnJumpPerformed += GetComponent<IJumpable>().OnJumpPressed;
        _wallClimbing = GetComponent<WallClimbing>();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _positionToSave = transform.position;
        _rotationToSave = _mainCamera.rotation;
    }

    private void FixedUpdate()
    {
        MoveVal = _playerInput.MoveVal;
        GroundCheck.CheckIfGrounded();

        MoveVal = _camMovement.CalculateMoveDirection(MoveVal, _playerInput);
        MoveVal = _camMovement.FaceMoveDirection(MoveVal, _playerRB);

        if (GroundCheck.Grounded) //Cursor.visible == false)
        {
            //TODO: When Grounded the Movement does not get into the ground fast enough
            _playerRB.velocity = _horizontalMovement.Move(MoveVal);
            //MovementOptionsCheck();
        }
        else if (_wallClimbing != null && _wallClimbing.IsClimbing)
        {

            //playerInput.OnJumpPerformed -= GetComponent<JumpingBehaviour>().OnJumpPressed;
            _playerRB.velocity = _wallClimbing.ClimbingMovement(-MoveVal);
        }
        else
        {
            _playerRB.velocity = _airborneMovement.CalculateMovement(MoveVal);
        }

    }
}
