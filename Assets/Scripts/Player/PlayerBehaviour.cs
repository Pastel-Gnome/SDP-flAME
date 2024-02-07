using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("public variables")]
    public LayerMask groundMask;
    public LayerMask lampMask;
    public Holdable heldObject;
    public float grabRadius;
    public Rigidbody rb;
    public Animator animator;
    public Transform orientation;
    public Collider capsuleCollider;
    public Vector3 movementInput;
    public Vector2 balance;
    public Transform plumbBob;
    public Transform plumbBobRoot;
    public PhysicMaterial groundFriction;
    public PhysicMaterial airFriction;

    [Header("player stats")]
    private PlayerState currentPlayerState;
    public float balanceRecoverRate;
    public float runSpeed;
    public float maxRunSpeed;
    public float jumpSpeed;
    public float jumpDuration;
    public float getupDuration;

    [Header("player bools")]
    public bool grounded;
    public bool jumping;

    // Start is called before the first frame update
    private void Start()
    {
        currentPlayerState = new State_Stand(this);
    }

    // Update is called once per frame
    private void Update()
    {
        //check if player is grounded
        bool groundedNew = Physics.Raycast(orientation.position, -orientation.up, 1.25f, groundMask);
        grounded = groundedNew;

        //get movement input
        movementInput = orientation.forward * Input.GetAxisRaw("Vertical") + orientation.right * Input.GetAxisRaw("Horizontal");

        //get jumping input
        if(Input.GetButtonDown("Jump") && grounded){ jumping = true; }

        //pick up lantern
        if(Input.GetButtonDown("Grab")){
            Transform closestGrab = null;
            Collider[] grabHits = Physics.OverlapSphere(orientation.position, grabRadius, lampMask);
            foreach(Collider i in grabHits){
                if(closestGrab == null || Vector3.Distance(i.transform.position, orientation.position) < Vector3.Distance(closestGrab.position, orientation.position)){
                    closestGrab = i.transform;
                }
            }
            heldObject = closestGrab.GetComponent<Holdable>();
            heldObject.gameObject.layer = 2;
        }

        //clamp horizontal speed
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        horizontalVelocity = horizontalVelocity.magnitude > maxRunSpeed ? Vector3.ClampMagnitude(horizontalVelocity, maxRunSpeed) : horizontalVelocity;
        rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);

        //set plumb bob rotation. might move this to a different script later, we'll see
        plumbBob.eulerAngles = new Vector3(balance.x, 90, balance.y);
        plumbBobRoot.localEulerAngles = new Vector3(0, plumbBobRoot.eulerAngles.y + 4, 0);
     
        balance = Vector2.ClampMagnitude(balance, 100);

        currentPlayerState.Update();
    }

    private void FixedUpdate() 
    {
        currentPlayerState.FixedUpdate();

        if(heldObject){heldObject.transform.position = orientation.position;}
    }

    public void SetPlayerState(PlayerState currentPlayerStateNew)
    {
        print(currentPlayerState + " -> " + currentPlayerStateNew);

        currentPlayerState?.OnExitState();

        currentPlayerState = currentPlayerStateNew;

        currentPlayerState.OnEnterState();
    }

    public void SetColliderMaterial(PhysicMaterial newPhysicsMaterial){
        capsuleCollider.material = newPhysicsMaterial;
    }
}
