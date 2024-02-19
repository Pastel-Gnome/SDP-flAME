using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehavior : MonoBehaviour
{
    private float maxLight = 5.0f;
    public float lightLevel = 5.0f;

    private Holdable lantern;
    public Light bulb;

    void Start()
    {
        lantern = GetComponent<Holdable>();
        bulb = GetComponentInChildren<Light>();
    }

    void FixedUpdate()
    {
        if (lantern.holder && lightLevel < maxLight){ lightLevel += Time.fixedDeltaTime; if (lightLevel == maxLight) { Debug.Log("Lantern Light Max"); } }
        if (!lantern.holder && lightLevel > 0) { lightLevel -= Time.fixedDeltaTime; if (lightLevel == 0) { Debug.Log("Lantern Light Zero"); } }

        bulb.intensity = lightLevel;
    }
}
