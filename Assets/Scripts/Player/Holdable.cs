using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    private Rigidbody rb;
    public Transform holder;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(holder){
            transform.SetPositionAndRotation(holder.position, holder.rotation);
        }
    }

    public virtual void grabbed(Transform holderNew){
        rb.isKinematic = true;
        holder = holderNew;
        gameObject.layer = 2;
    }

    public virtual void dropped(Vector3 exitForce){
        rb.isKinematic = false;
        rb.AddForce(exitForce);
        holder = null;
        gameObject.layer = 7;
    }
}
