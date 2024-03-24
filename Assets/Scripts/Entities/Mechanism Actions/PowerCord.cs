using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCord : MechanismAction
{
    [SerializeField] private LineRenderer lineRenderer;
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

        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.Lerp(colorA, colorB, currentPower), 0.0f)},
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 1)}
        );
        lineRenderer.colorGradient = gradient;
    }
}
