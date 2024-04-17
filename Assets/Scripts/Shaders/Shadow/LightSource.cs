using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float currentRange, maxRange;
    public float decay = 0.01f;
    public Holdable holdable;
    [SerializeField] private float chargeRange = 2;
    [SerializeField] private Light decorLight;
    [SerializeField] private MeshRenderer decorEmissive;
    private float maxDecorLightLevel;

    private void Awake(){
        LightManager.i.lightSources.Add(this);
    }

    private void Start() {
        TryGetComponent(out Holdable newHoldable);
        if(decorLight){
            maxDecorLightLevel = decorLight.intensity;
        }
        
        holdable = newHoldable;
    }

    public void Charge(Transform[] chargeSources){
        bool foundChargeSource = false;
        if(holdable && holdable.holder){
            holdable.holder.currentPower = Mathf.Lerp(holdable.holder.currentPower, currentRange/maxRange, 0.25f);
            //print(holdable.holder.currentPower + ", " + holdable.holder.name);
            
            if(holdable.holder.providesCharge){
                foundChargeSource = true;
            }
        }

        foreach(Transform j in chargeSources){
            if(Vector3.Distance(transform.position, j.position) < chargeRange){
                foundChargeSource = true;
                break;
            }
        }

        currentRange = foundChargeSource ? Mathf.Lerp(currentRange, maxRange, 0.01f) : currentRange -= decay;
        currentRange = currentRange < 0 ? 0 : currentRange;
        if(decorLight){
            decorLight.intensity = Mathf.Lerp(0f, maxDecorLightLevel, currentRange/maxRange);
        }
        if(decorEmissive){
            decorEmissive.material.color = Color.Lerp(Color.black, Color.white, currentRange/maxRange);
        }
    }
}
