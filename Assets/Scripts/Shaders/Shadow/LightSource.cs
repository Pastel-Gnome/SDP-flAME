using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float currentRange, maxRange;
    public float decay = 0.01f;
    public Holdable holdable;

    private void Start() {
        TryGetComponent(out Holdable newHoldable);
        holdable = newHoldable;
    }
}
