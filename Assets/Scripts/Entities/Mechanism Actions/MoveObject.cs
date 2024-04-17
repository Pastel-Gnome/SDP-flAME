using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MechanismAction
{
    [SerializeField] private Transform positionA, positionB;
    private Transform currentTarget;
    public float moveSpeed;
    public Transform moveObjectTransform;
    public Vector3 currentVelocity;
    private AudioSource audioSource;
    private float maxVolume;
    
    // Start is called before the first frame update
    public override void OnStart(Mechanism mechanism)
    {
        TryGetComponent(out AudioSource audioSourceNew);
        audioSource = audioSourceNew;
        if(audioSource){
            maxVolume = audioSource.volume;
        }
    }

    // Update is called once per frame
    public override void OnUpdate(Mechanism mechanism)
    {
        
    }

    // Update is called once per frame
    public override void OnFixedUpdate(Mechanism mechanism)
    {
        base.OnFixedUpdate(mechanism);

        Vector3 targetPosition = Vector3.Lerp(positionA.localPosition, positionB.localPosition, currentPower);
        moveObjectTransform.localPosition = Vector3.SmoothDamp(moveObjectTransform.localPosition, targetPosition, ref currentVelocity, 0, moveSpeed);
        if(audioSource){
            audioSource.volume = Mathf.Lerp(audioSource.volume, Mathf.Min((float)Math.Round(currentVelocity.magnitude, 2) * 10, maxVolume), 0.25f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.TryGetComponent(out Rigidbody rbNew) && !rbNew.isKinematic){
            rbNew.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.TryGetComponent(out Rigidbody rbNew) && !rbNew.isKinematic){
            rbNew.transform.SetParent(null);
        }
    }
}
