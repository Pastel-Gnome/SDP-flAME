using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    private Rigidbody rb;
    public Holder holder;
    [SerializeField] private Vector3 centerOfMass;
    private bool canBeGrabbed;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
		rb = GetComponent<Rigidbody>();
		rb.centerOfMass = centerOfMass;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if(holder){
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, holder.carryAnchor.position, holder.carryLerpRate), holder.carryAnchor.rotation);
        }
    }

    public virtual void grabbed(Holder holderNew){
        if(holder){holder.OnRelease();}
        holder = holderNew;
        holder.OnGrab();
        
        canBeGrabbed = holder.canBeGrabbed;

        if (!canBeGrabbed) { gameObject.layer = 2; }
        else { gameObject.layer = 7; }
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    public virtual void dropped(Vector3 exitForce){
        if(holder){holder.OnRelease();}
        holder = null;

        gameObject.layer = 7;
        rb.useGravity = true;
        rb.isKinematic = false;

        rb.AddForce(exitForce);
    }

    public bool GetGrabAbility()
    {
        return canBeGrabbed;
    }

	public virtual void loadUnheld()
	{
		rb.isKinematic = false;
		holder = null;
		gameObject.layer = 7;
		rb.useGravity = true;
	}
}
