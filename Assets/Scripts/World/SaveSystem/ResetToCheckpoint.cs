using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetToCheckpoint : MonoBehaviour
{
	static PlayerBehaviour playerBehaviour;
	private void Start()
	{
		if (playerBehaviour == null)
		{
			playerBehaviour = FindFirstObjectByType<PlayerBehaviour>();
		}
	}

	// This script is used on the "kill box", which is usually in dangerous areas like a steep drop
	private void OnTriggerEnter(Collider other)
	{
        if (other.name == "Player")
        {
			Debug.Log("Kill box entered");
			StartCoroutine(other.GetComponent<PlayerBehaviour>().Die()); // Change if death animation different from darkness and falling death
		} else if (other.gameObject.layer == 7) // if the object is in the "Holdable" layer, and is therefore a lantern.
		{
			Debug.Log("Lantern fell into killbox.");
			StartCoroutine(playerBehaviour.Die()); // Change if death animation different from darkness and falling death
		}
	}
}
