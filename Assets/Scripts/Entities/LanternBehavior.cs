using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehavior : MonoBehaviour
{

    private bool isPlaced = false;
    private Holdable lantern;
    private LightSource lightSource;
    private SphereCollider lightArea;

	static private int lightTouchNum;

    void Start()
    {
        lantern = GetComponent<Holdable>();
        lightSource = GetComponent<LightSource>();
        lightArea = GetComponentInChildren<SphereCollider>();
    }

	private void FixedUpdate()
	{
        if(lightSource.currentRange != 0f)
        {
			lightArea.radius = lightSource.currentRange + 0.5f;
		} else
        {
            lightArea.radius = 0f;
        }
	}

	private void OnTriggerStay(Collider other)
	{
        if (lantern.holder == null && isPlaced == false)
        {
            if (other.CompareTag("Holster"))
            {
				lantern.grabbed(other.transform.parent, true);
			} else if (other.CompareTag("Pedestal"))
			{
				lantern.grabbed(other.transform.parent, true);
			}

		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player")
		{
			lightTouchNum++;
			if(lightTouchNum > 0)
			{
				Debug.Log("Player in the light");
				other.GetComponent<PlayerBehaviour>().isLit = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.name == "Player")
		{
			lightTouchNum--;
			if (lightTouchNum == 0)
			{
				Debug.Log("Player outside of light");
				other.GetComponent<PlayerBehaviour>().isLit = false;
			} else if (lightTouchNum < 0)
			{ Debug.LogError("Somehow subtracted more lantern lights than should exist"); }
		}
	}
}
