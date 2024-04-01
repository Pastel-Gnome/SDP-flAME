using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishLight : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player")
		{
			Holdable heldObject = other.GetComponent<PlayerBehaviour>().heldObject;
			if (heldObject)
			{
				heldObject.GetComponent<LightSource>().currentRange = 0;
			}
		} else if (other.gameObject.layer == 7) // if the object is in the "Holdable" layer, and is therefore a lantern.
		{
			Debug.Log("Lantern fell into water.");
			other.GetComponent<LightSource>().currentRange = 0;
		}
	}
}
