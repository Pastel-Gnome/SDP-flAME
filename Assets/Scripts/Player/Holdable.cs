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
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(holder){
            transform.SetPositionAndRotation(holder.carryAnchor.position, holder.carryAnchor.rotation);
        }
    }

    public virtual void grabbed(Holder holderNew){
        if(holder){holder.holding = false;}
        holder = holderNew;
        holder.holding = true;
        canBeGrabbed = holder.canBeGrabbed;
        rb.isKinematic = true;
        holder = holderNew;
        if (!canBeGrabbed) { gameObject.layer = 2; }
        else { gameObject.layer = 7; }
        rb.useGravity = false;
    }

    public virtual void dropped(Vector3 exitForce){
        holder.holding = false;
        rb.isKinematic = false;
        rb.AddForce(exitForce);
        holder = null;
        gameObject.layer = 7;
        rb.useGravity = true;
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
