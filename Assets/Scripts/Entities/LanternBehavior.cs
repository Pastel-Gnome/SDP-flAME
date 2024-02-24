using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehavior : MonoBehaviour
{

    private bool isPlaced = false;
    private Holdable lantern;

    void Start()
    {
        lantern = GetComponent<Holdable>();
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
}
