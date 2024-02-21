using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehavior : MonoBehaviour
{
    // NOTE: If the light is not working, make sure the LanternBehavior script is on, the Point Light gameobject in the lantern heirarchy is enabled, and the Point Light's light component is enabled

    // If you want to turn off the light, please ensure that you disable the LanternBehavior script and disable the Point Light gameobject in the lantern heirarchy

    [SerializeField] float maxLight = 5.0f;
    public float lightLevel;
    private bool isFinalPlace = false;

    private Holdable lantern;
    public Light bulb;

    void Start()
    {
        //lightLevel = maxLight;

        lantern = GetComponent<Holdable>();
        bulb = GetComponentInChildren<Light>();

        if (!bulb) { Debug.LogError("Light gameobject or component not enabled"); }
    }

    void FixedUpdate()
    {
        if (!isFinalPlace)
        {
            if (lantern.holder && lightLevel < maxLight) { lightLevel += Time.fixedDeltaTime; }
            if (!lantern.holder && lightLevel > 0) { lightLevel -= Time.fixedDeltaTime; }

            bulb.intensity = lightLevel;
        } else
        {
			if (lantern.holder && lightLevel < maxLight) { lightLevel += Time.fixedDeltaTime; if (lightLevel == maxLight) { this.enabled = false; } }
		}
    }

	private void OnTriggerStay(Collider other)
	{
        if (other.CompareTag("Pedestal") && lantern.holder == null && isFinalPlace == false)
        {
            lantern.grabbed(other.transform.parent);
            isFinalPlace = true;
        }
	}
}
