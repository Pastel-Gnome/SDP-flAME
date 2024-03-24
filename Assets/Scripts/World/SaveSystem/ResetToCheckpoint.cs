using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetToCheckpoint : MonoBehaviour
{
	// This script is used on the "kill box", which is usually in dangerous areas like a steep drop
	private void OnTriggerEnter(Collider other)
	{
        if (other.name == "Player")
        {
			Debug.Log("Kill box entered");
			StartCoroutine(other.GetComponent<PlayerBehaviour>().Die()); // Change if death animation different from darkness and falling death
		}
	}
}
