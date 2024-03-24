using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanism : MonoBehaviour
{
    [SerializeField] private PowerSource[] powerSources;
    [SerializeField] private MechanismAction[] mechanismActions;
    [SerializeField] private float maxPowerInput;
    public float currentPowerInput;

    // Start is called before the first frame update
    private void Start()
    {
        foreach(MechanismAction i in  mechanismActions){
            i.OnStart(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        foreach(MechanismAction i in  mechanismActions){
            i.OnFixedUpdate(this);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float totalInput = 0;
        foreach(PowerSource i in powerSources){totalInput += i.currentPower;}
        currentPowerInput = totalInput;

        foreach(MechanismAction i in mechanismActions){
            i.OnFixedUpdate(this);
        }
    }

    private void Activate(){

    }

    private void Deactivate(){

    }
}
