using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    private Rigidbody rb;
    public Transform holder;
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
            transform.SetPositionAndRotation(holder.position, holder.rotation);
        }
    }

    public virtual void grabbed(Transform holderNew, bool canBeGrabbedNew){
        canBeGrabbed = canBeGrabbedNew;
        rb.isKinematic = true;
        holder = holderNew;
        if (!canBeGrabbed) { gameObject.layer = 2; }
        else { gameObject.layer = 7; }
        rb.useGravity = false;
    }

    public virtual void dropped(Vector3 exitForce){
        rb.isKinematic = false;
        rb.AddForce(exitForce);
        holder = null;
        gameObject.layer = 7;
        rb.useGravity = true;
    }
}
