using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanismAction : MonoBehaviour
{
    [SerializeField] private float requiredPower = 1;
    [SerializeField] protected float currentPower;
    public virtual void OnStart(Mechanism mechanism){

    }

    public virtual void OnUpdate(Mechanism mechanism){

    }

    public virtual void OnFixedUpdate(Mechanism mechanism){
        currentPower = mechanism.currentPowerInput/requiredPower;
    }
}
