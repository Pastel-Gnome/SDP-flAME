using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBulb : MechanismAction
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Color colorA, colorB;
    
    // Start is called before the first frame update
    public override void OnStart(Mechanism mechanism)
    {
        
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
    }
}
