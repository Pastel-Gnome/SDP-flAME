using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	int myCheckpoint;

	private void Start()
	{
		myCheckpoint = transform.GetSiblingIndex();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player")
		{
			Debug.Log("Checkpoint entered");
			SaveManager.SaveJsonData(myCheckpoint);
		}
	}
}
