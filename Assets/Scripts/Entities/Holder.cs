using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : PowerSource
{
    public Mechanism[] powerOutput;
    public bool canBeGrabbed;
    public bool providesCharge;
    public Transform carryAnchor;
    public Holdable holding;

    private void Start(){
        if(!carryAnchor){
            carryAnchor = transform;
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.TryGetComponent(out Holdable holdableNew) && !holding){
            holdableNew.grabbed(this);
        }
    }

    private void FixedUpdate(){
        if(!holding){
            currentPower = Mathf.Lerp(currentPower, 0, 0.25f);
        }
    }
}
