using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check")]
    private float _rayDepth = 0.1f;
    [SerializeField]
    private LayerMask _whatIsGround;
    public bool Grounded { get; private set; }
    private RaycastHit _hitInfo;
    //private Transform transform;
    private CapsuleCollider _capsuleCollider;

    private void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
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
        var newPosition = new Vector3(transform.position.x, transform.position.y - (_capsuleCollider.height / 2), transform.position.z);


        return newPosition;
    }

    public void CheckIfGrounded()
    {
        //Start Ray from within the Collider to detect the layer everytime even when the collider and layer interfere with each other
        var ray = new Ray(GetFeetPositionOfCollider() + Vector3.up * 0.01f, Vector3.down);

        Grounded = Physics.Raycast(ray, out _hitInfo, _rayDepth, _whatIsGround);
        Debug.DrawRay(ray.origin, ray.direction * _rayDepth, Color.red, 1f);
    }
}
