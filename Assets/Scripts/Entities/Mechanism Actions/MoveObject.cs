using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MechanismAction
{
    [SerializeField] private Vector3 positionA, positionB;
    public float moveSpeed;
    public Transform moveObjectTransform;
    private Vector3 currentVelocity;
    
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

        Vector3 targetPosition = Vector3.Lerp(positionA, positionB, currentPower);
        moveObjectTransform.localPosition = Vector3.SmoothDamp(moveObjectTransform.localPosition, targetPosition, ref currentVelocity, moveSpeed * Time.fixedDeltaTime);
    }
}
