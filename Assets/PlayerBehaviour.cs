using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("public variables")]
    public LayerMask groundMask;
    public Rigidbody rb;
    public Animator animator;
    public Transform orientation;
    public Collider capsuleCollider;

    private Vector3 movementInput;
    [SerializeField] private float runSpeed;
    [SerializeField] private float maxRunSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpDuration;
    [SerializeField] private PhysicMaterial groundFriction;
    [SerializeField] private PhysicMaterial airFriction;
    private bool grounded;
    private bool jumping;
    private bool stumbling;
    [SerializeField] private float jumpTimeElapsed;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        movementInput = orientation.forward * Input.GetAxisRaw("Vertical") + orientation.right * Input.GetAxisRaw("Horizontal");

        bool groundedNew = Physics.Raycast(orientation.position, -orientation.up, 1.25f, groundMask);
        if(groundedNew && !grounded){
            StartCoroutine(Stumble(0.5f, new Vector3(rb.velocity.x, 0, rb.velocity.y)));
        }
        grounded = groundedNew;

        capsuleCollider.material = grounded && !stumbling ? groundFriction : airFriction;

        Jump();
    }

    private void FixedUpdate() {

        //run
        rb.AddForce(movementInput.normalized * runSpeed * (grounded ? 1 : 0.25f), ForceMode.Force);
        
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        horizontalVelocity = horizontalVelocity.magnitude > maxRunSpeed ? Vector3.ClampMagnitude(horizontalVelocity, maxRunSpeed) : horizontalVelocity;
        rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);

        RotateToMovement(8);

        //jump
        if(jumping){
            rb.AddForce(orientation.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    private void RotateToMovement(float rotationSpeed = 10){
        if(!grounded){
            animator.Play("Jump");
        }
        else if(movementInput != Vector3.zero){
            animator.transform.forward = Vector3.Slerp(animator.transform.forward, movementInput, Time.deltaTime * rotationSpeed);
            animator.Play("Run");
        }
        else{
            animator.Play("Stand");
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(orientation.position, orientation.position - new Vector3(0, 1, 0));
    }

    private void Jump(){
        if(grounded && !jumping && !Input.GetButton("Jump")){
            //print("not jumping");
            jumpTimeElapsed = 0;
        }
        else if(Input.GetButton("Jump") && jumpTimeElapsed < jumpDuration){
            //print("jumping");
            jumping = true;
            jumpTimeElapsed += Time.deltaTime;
        }       
        else{
            //print("end jump");
            jumping = false;
            jumpTimeElapsed = jumpDuration;
        }
    }

    private IEnumerator Stumble(float stumbleDuration, Vector3 stumbleInput){
        float stumbleElapsed = 0;
        stumbling = true;

        while(stumbleElapsed < stumbleDuration){
            if(grounded){
                stumbleElapsed += Time.fixedDeltaTime;
            }

            yield return new WaitForFixedUpdate();
        }

        stumbling = false;
    }
}
