using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetToCheckpoint : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
        if (other.name == "Player")
        {
			Debug.Log("Kill box entered");
			SaveManager.LoadJsonData();
		}
	}
}
