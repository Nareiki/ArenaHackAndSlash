using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check")]
    private float rayDepth = 0.1f;
    [SerializeField]
    private LayerMask whatIsGround;
    public bool grounded { get; private set; }
    private RaycastHit hitInfo;
    //private Transform transform;
    private CapsuleCollider capsuleCollider;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        //whatIsGround = LayerMask.NameToLayer("Ground");//(1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Ground"));
    }

    private void FixedUpdate()
    {
        CheckIfGrounded();
    }

    /*public GroundCheck(Transform transform, CapsuleCollider capsuleCollider)
    {
        this.transform = transform;
        this.capsuleCollider = capsuleCollider;
        whatIsGround = LayerMask.NameToLayer("Ground");//(1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Ground"));
    }*/

    private Vector3 GetFeetPositionOfCollider()
    {
        var newPosition = new Vector3(transform.position.x, transform.position.y - (capsuleCollider.height / 2), transform.position.z);


        return newPosition;
    }

    public void CheckIfGrounded()
    {
        //Start Ray from within the Collider to detect the layer everytime even when the collider and layer interfere with each other
        var ray = new Ray(GetFeetPositionOfCollider() + Vector3.up * 0.01f, Vector3.down);

        grounded = Physics.Raycast(ray, out hitInfo, rayDepth, whatIsGround);
        Debug.DrawRay(ray.origin, ray.direction * rayDepth, Color.red, 1f);
    }
}
