using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicJump : MonoBehaviour, IJumpable
{
    Rigidbody playerRB;

    [Header("Jump Variables")]
    [SerializeField] public float Jumpforce = 10f;
    public bool JumpUsed { get; private set; }
    private ICoyoteCheckable _coyoteCheck;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        _coyoteCheck = GetComponent<ICoyoteCheckable>();
    }

    #region Jump Handling

    public virtual void OnJumpPressed()
    {
        if (_coyoteCheck != null)
        {
            if (!JumpUsed && _coyoteCheck.StateCheck())
            {
                DoJumping();
                return;
            }
            return;
        }
        DoJumping();
    }

    public virtual void DoJumping()
    {
        playerRB.velocity = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);
        playerRB.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
        JumpUsed = true;
        //audioSource.PlayOneShot(jumpAudio);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        JumpUsed = false;
    }

    public virtual void OnCollisionExit(Collision other)
    {
        if (_coyoteCheck == null)
        {
            JumpUsed = true;
        }
    }
    #endregion
}
