using Cinemachine;
using System;
using System.Collections;
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
    private PlayerState currentPlayerState;
    public AudioSource audioSource;
	[Header("player stats")]
	public float balanceRecoverRate;
    public float runSpeed;
    public float maxRunSpeed;
    public float jumpSpeed;
    public int jumpImpulsesMax;
    public float dropSpeed;
    public float getupDuration;
    public float grabTime;
    public float maxShadowTime = -999;
    public float shadowTimer;

    [Header("player bools")]
    public RaycastHit grounded;
    public bool jumping;
    public bool grabbing;
    public bool isLit;
    public bool isInCutscene;

    [Header("player sounds")]
    public AudioClip[] footstepsAudio;
    public AudioClip[] jumpAudio;
    public AudioClip[] landAudio;
    public AudioClip[] grabAudio;
    public float stepTimer;
    private bool connectedWithGround;
    private float currentMagnitude;
    public bool intro;
    public float introShadow;

    // Start is called before the first frame update
    private void Start()
    {
        currentPlayerState = new State_Stand(this);
        currentPlayerState.Chosen();

        audioSource = GetComponent<AudioSource>();
        CinemachineFreeLook playerCamera = this.transform.parent.GetComponentInChildren<CinemachineFreeLook>();
        playerCamera.m_XAxis.m_InvertInput = SaveManager.instance.mouseInvertX;
		playerCamera.m_YAxis.m_InvertInput = SaveManager.instance.mouseInvertY;
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
        animator.SetBool("grounded", grounded.collider);
        animator.transform.forward = Vector3.Slerp(animator.transform.forward, new Vector3(movementInput.x, 0, movementInput.z), Time.deltaTime * 10);
        
        float shadowPercent;

        if(intro){
            shadowPercent = 2.5f;
        }
        else if(introShadow > 0){
            introShadow -= Time.deltaTime;
            shadowPercent = introShadow;
        }
        else{
            shadowPercent = (maxShadowTime - shadowTimer) * 2/maxShadowTime;
        }
        
        AudioManager.i.SetDangerLevel(shadowPercent);
        Shader.SetGlobalFloat("_PlayerShadowLevel", shadowPercent);
        

        //check if player is lit
        isLit = LightManager.i.CalculateLightAtPoint(rb.transform.position) < 1f;
        //isLit = true;
        //print(LightManager.i.CalculateLightAtPoint(rb.transform.position));
        
        if (!isLit && maxShadowTime != -999 && shadowTimer > 0){
            shadowTimer -= Time.deltaTime;
            if (!intro && shadowTimer <= 0) { 
                Debug.Log("Player has been in darkness too long. Game Over"); 
                StartCoroutine(Die(0.5f)); 
                shadowTimer = 0; 
            }
        } else if (isLit && shadowTimer < maxShadowTime && shadowTimer != 0) {
			shadowTimer += Time.deltaTime;
		}

        //
        //Shader.SetGlobalFloatArray("_ShadowLevel", shadowLevel);

        //check if player is grounded
        Physics.SphereCast(rb.position, 0.25f, -rb.transform.up, out RaycastHit hit, 1.3f, groundMask);
        grounded = hit;

        if (!isInCutscene)
        {
            //get jumping input
            if (Input.GetButtonDown("Jump") && grounded.collider) { jumping = true; }

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
            currentMagnitude = directionalInputs.magnitude > 0 ? Mathf.Lerp(currentMagnitude, directionalInputs.magnitude, 0.05f): Mathf.Lerp(currentMagnitude, directionalInputs.magnitude, 0.5f);
            currentMagnitude = currentMagnitude < 0.05f ? 0 : currentMagnitude;
            print("current magnitude: " + currentMagnitude);
            movementInput = Vector3.Slerp(movementInput, directionalInputs, 0.25f).normalized * currentMagnitude;
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
        Collider[] grabHits = Physics.OverlapSphere(rb.position, grabRadius, holdableMask);
        foreach(Collider i in grabHits){
            if(closestGrab == null || Vector3.Distance(i.transform.position, rb.position) < Vector3.Distance(closestGrab.position, rb.position)){
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
        else if(new Vector2(rb.velocity.x, rb.velocity.z).magnitude > 0.1f){
            timeSinceLastStep = 0;
            audioSource.PlayOneShot(footstepsAudio[UnityEngine.Random.Range(0, footstepsAudio.Length)]);
        }
        return timeSinceLastStep;
    }

    private void OnCollisionEnter(Collision other) {
        connectedWithGround = true;
    }

    private void OnCollisionExit(Collision other) {
        connectedWithGround = false;
    }
}
