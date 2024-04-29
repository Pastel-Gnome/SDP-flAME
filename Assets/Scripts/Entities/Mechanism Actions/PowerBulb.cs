using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBulb : MechanismAction
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Light[] affectedLights;
    [SerializeField] private LightSource[] affectedMechanicalLights;
    [SerializeField] float[] affectedLightStartValues;
    [SerializeField] private Color colorA, colorB;
    
    // Start is called before the first frame update
    public override void OnStart(Mechanism mechanism)
    {
        affectedLightStartValues = new float[affectedLights.Length];
        for(int i = 0; i < affectedLights.Length; i++){
            affectedLightStartValues[i] = affectedLights[i].intensity;
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

        meshRenderer.material.color = Color.Lerp(colorA, colorB, currentPower);
        for(int i = 0; i < affectedLights.Length; i++){
            affectedLights[i].intensity = Mathf.Lerp(0, affectedLightStartValues[i], currentPower);
        }

        for(int i = 0; i < affectedMechanicalLights.Length; i++){
            affectedMechanicalLights[i].currentRange = Mathf.Lerp(0, affectedMechanicalLights[i].maxRange, currentPower);
        }
    }
}
