using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float currentRange, maxRange;
    private Holdable holdable;
    [SerializeField] private float decay = 0.01f;

    private void Start() {
        TryGetComponent(out Holdable newHoldable);
        holdable = newHoldable;

        if(holdable){
            StartCoroutine(HoldableUpdate());
        }
    }

    private IEnumerator HoldableUpdate() {
        while(holdable){
            currentRange = holdable.holder ? Mathf.Lerp(currentRange, maxRange, 0.25f) : currentRange -= decay;
            currentRange = currentRange < 0 ? 0 : currentRange;
            
            yield return new WaitForFixedUpdate();
        }
    }
}
