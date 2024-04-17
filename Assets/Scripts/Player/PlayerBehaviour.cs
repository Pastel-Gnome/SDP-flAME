using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("public variables")]
    public LayerMask groundMask;
    public LayerMask holdableMask;
    public Holdable heldObject;
    public float grabRadius;
    public Rigidbody rb;
    public Animator animator;
    public Transform orientation;
    public Transform placePosition;
    public Collider capsuleCollider;
    public Vector3 movementInput;
    public Vector2 balance;
    public Transform plumbBob;
    public Transform plumbBobRoot;
    public PhysicMaterial groundFriction;
    public PhysicMaterial airFriction;
    [SerializeField] private Holder holder;
    private Slider TempDarknessIndicator;
    private PlayerState currentPlayerState;
    public AudioSource audioSource;
	[Header("player stats")]
	public float balanceRecoverRate;
    public float runSpeed;
    public float maxRunSpeed;
    public float jumpSpeed;
    public float jumpDuration;
    public float dropSpeed;
    public float getupDuration;
    public float grabTime;
    public float maxShadowTime = -999;
    public float shadowTimer;

    [Header("player bools")]
    public bool grounded;
    public bool jumping;
    public bool grabbing;
    public bool isLit;
    public bool isInCutscene;

    [Header("player sounds")]
    public AudioClip[] footstepsAudio;
    public float stepTimer;

    // Start is called before the first frame update
    private void Start()
    {
        currentPlayerState = new State_Stand(this);
        currentPlayerState.Chosen();

        TempDarknessIndicator = plumbBob.parent.GetComponentInChildren<Slider>();
        if(maxShadowTime == -999)
        {
            TempDarknessIndicator.gameObject.SetActive(false);
        }
        grounded = true;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontalSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        if(horizontalSpeed < 0.05f){
            horizontalSpeed = 0;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        animator.SetFloat("Horizontal_Speed", horizontalSpeed);
        animator.SetBool("grounded", grounded);
        animator.transform.forward = Vector3.Slerp(animator.transform.forward, movementInput, Time.deltaTime * 10);

        float shadowPercent = (maxShadowTime - shadowTimer) * 2/maxShadowTime;
        AudioManager.i.SetDangerLevel(shadowPercent);
        Shader.SetGlobalFloat("_PlayerShadowLevel", shadowPercent);

        //check if player is lit
        isLit = LightManager.i.CalculateLightAtPoint(rb.transform.position) < 1f;
        //isLit = true;
        //print(LightManager.i.CalculateLightAtPoint(rb.transform.position));
        
        if (!isLit && maxShadowTime != -999 && shadowTimer > 0){
            shadowTimer -= Time.deltaTime;
            if (shadowTimer <= 0) { 
                Debug.Log("Player has been in darkness too long. Game Over"); 
                StartCoroutine(Die(0.5f)); 
                shadowTimer = 0; 
            }
        } else if (isLit && shadowTimer < maxShadowTime && shadowTimer != 0) {
			shadowTimer += Time.deltaTime;
		}

        if (maxShadowTime != -999){
            TempDarknessIndicator.value = shadowTimer / maxShadowTime; // slider value is a fraction of maxShadowTime, showing what % of time is left
        }
        //
        //Shader.SetGlobalFloatArray("_ShadowLevel", shadowLevel);

        //check if player is grounded
        bool groundedNew = Physics.SphereCast(orientation.position, 0.25f, -orientation.up, out RaycastHit hit, 1.25f, groundMask);
        grounded = groundedNew;

        if (!isInCutscene)
        {
            //get jumping input
            if (Input.GetButtonDown("Jump") && grounded) { jumping = true; }

            //pick up lantern
            if (Input.GetButtonDown("Grab")) { grabbing = true; }

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
    }

    private void FixedUpdate() 
    {
        if (!isInCutscene)
        {
            //get movement input
            Vector3 directionalInputs = orientation.forward * Input.GetAxisRaw("Vertical") + orientation.right * Input.GetAxisRaw("Horizontal");
            movementInput = Vector3.Lerp(movementInput, directionalInputs, 0.25f).normalized * directionalInputs.magnitude;
        }

        currentPlayerState.FixedUpdate();
    }

    public void SetPlayerState(PlayerState currentPlayerStateNew)
    {
        //print(currentPlayerState + " -> " + currentPlayerStateNew);

        currentPlayerState?.OnExitState();

        currentPlayerState = currentPlayerStateNew;

        currentPlayerState.OnEnterState();
    }

    public void SetColliderMaterial(PhysicMaterial newPhysicsMaterial){
        capsuleCollider.material = newPhysicsMaterial;
    }

    public void RecoverBalance(float recoveryMod = 1){
        /*
        balance = new Vector2(
            balance.x > 0.2 ? balance.x - balanceRecoverRate : balance.x + balanceRecoverRate, 
            balance.y > 0.2 ? balance.y - balanceRecoverRate : balance.y + balanceRecoverRate
        );
        */

        balance = Vector3.Slerp(balance, Vector2.zero, balanceRecoverRate * recoveryMod);
        //balance = Vector2.SmoothDamp(balance, Vector2.zero, ref balanceRecoveryVelocity, 0, player.balanceRecoverRate * recoveryMod);
    }

    public IEnumerator Grab(float delay = 0){
        yield return new WaitForSeconds(delay);

        Transform closestGrab = null;
        Collider[] grabHits = Physics.OverlapSphere(orientation.position, grabRadius, holdableMask);
        foreach(Collider i in grabHits){
            if(closestGrab == null || Vector3.Distance(i.transform.position, orientation.position) < Vector3.Distance(closestGrab.position, orientation.position)){
                closestGrab = i.transform;
            }
        }

        if(closestGrab){
            heldObject = closestGrab.GetComponent<Holdable>();
            heldObject.grabbed(holder);
        }

        animator.SetBool("Carrying", heldObject);
    }

    public IEnumerator Drop(bool placed, Vector3 exitForce, float delay = 0){
        yield return new WaitForSeconds(delay);

        if(heldObject){
            heldObject.dropped(exitForce);
            heldObject = null;
        }
    }

    public IEnumerator Die(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        SaveManager.LoadJsonData();
    }

    public float Step(float timeSinceLastStep){
        if(timeSinceLastStep < stepTimer){
            timeSinceLastStep += Time.fixedDeltaTime;
        }
        else{
            timeSinceLastStep = 0;
            audioSource.PlayOneShot(footstepsAudio[UnityEngine.Random.Range(0, footstepsAudio.Length)]);
        }
        return timeSinceLastStep;
    }
}
