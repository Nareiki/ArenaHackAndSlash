using System.Collections;
using UnityEngine;

[System.Obsolete("Already Splitted for more Clarity", true)]
public class JumpingBehaviour : MonoBehaviour
{
    [Header("Jump Variables")]
    [SerializeField] public float Jumpforce = 10f;
    [SerializeField] public bool MultiJumpEnabled = true;
    private bool _isJumping;
    private Rigidbody _playerRB;

    [SerializeField] public int MaxJumpAmount = 2;
    [SerializeField] private float _coyoteTime = 0.5f;
    //[SerializeField] private float jumpBuffer = 0.5f;

    //[SerializeField] private float jumpResetTimer = 1f;
    private int _jumpCount;

    private float _coyoteTimeCounter;
    //private bool inputBufferRestart;
    private float _lastJumpPressed;
    private bool _jumpCoroutineIsRunning;

    private void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CoyoteTimeAndDoubleJumpCheck();
    }

    #region Jump Handling

    public void OnJumpPressed()
    {
        _lastJumpPressed = Time.time;

        if (_coyoteTimeCounter < 0f && _jumpCount == MaxJumpAmount && _isJumping)
            _jumpCount--;

        if (_coyoteTimeCounter > 0f && _jumpCount == MaxJumpAmount && !_isJumping)
        {
            DoJumping();
            Debug.Log("Test, why does this work? " + _jumpCount);
        }
        else if (MultiJumpEnabled && _jumpCount > 0 && _jumpCount < MaxJumpAmount && _isJumping)
        {
            DoJumping();
            Debug.Log("Test, why does this work? " + _jumpCount);

        }

        //TODO: Implement Input Buffer
    }

    private void DoJumping()
    {
        _playerRB.velocity = new Vector3(_playerRB.velocity.x, 0, _playerRB.velocity.z);
        _playerRB.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
        _jumpCount--;
        _isJumping = true;
        //audioSource.PlayOneShot(jumpAudio);
    }

    #endregion
    private void CoyoteTimeAndDoubleJumpCheck()
    {
        if (!_isJumping)
        {
            _coyoteTimeCounter = _coyoteTime;
            _jumpCount = MaxJumpAmount;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        _isJumping = false;
    }

    /// <summary>
    /// Set the is Jumping to true so you are able to double Jump
    /// When JumpCount is less then the max Jump Count minus 1, reset Jumps (The Jump will subtract 1 automatically if the Coyote Time is less then 0, so there will almost always be 1 less Jump available then maxAmount)
    /// </summary>
    /// <param name="collision">The Object the Player is colliding with (all Layers)</param>
    private void OnCollisionExit(Collision collision)
    {
        _isJumping = true;

        if (_jumpCount < MaxJumpAmount - 1 && !_jumpCoroutineIsRunning)
        {
            Debug.Log("Starting Coroutine!");
            StartCoroutine(JumpReset());
        }
    }

    private IEnumerator JumpReset()
    {
        _jumpCoroutineIsRunning = true;
        _jumpCount = MaxJumpAmount;
        yield return new WaitForSeconds(1);
        _jumpCoroutineIsRunning = false;
    }
}
