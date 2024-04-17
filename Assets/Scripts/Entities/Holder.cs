
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : PowerSource
{
    public bool canBeGrabbed;
    public bool providesCharge;
    public Transform carryAnchor;
    public bool holding;
    public float carryLerpRate = 0.25f;
    [SerializeField] private AudioClip[] grabNoise;
    [SerializeField] private AudioClip[] releaseNoise;
    AudioSource audioSource;


    private void Start(){
        audioSource = GetComponent<AudioSource>();
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

    public void OnGrab(){
        holding = true;

        if(grabNoise.Length > 0){
            audioSource.PlayOneShot(grabNoise[Random.Range(0, grabNoise.Length)]);
        }
    }

    public void OnRelease(){
        holding = false;

        if(releaseNoise.Length > 0){
            audioSource.PlayOneShot(releaseNoise[Random.Range(0, releaseNoise.Length)]);
        }
    }
}
