using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep : MonoBehaviour
{
	public int thisStep;
	private TutorialUI ui;
	private void Start()
	{
		ui = transform.parent.GetComponent<TutorialUI>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player")
		{
			if (thisStep == 0)
			{
				ui.StartWalkText();
			}
			else if (thisStep == 1)
			{
				ui.StartInteractText();
			}
			else if (thisStep == 2)
			{
				ui.StartJumpText();
			}
			Destroy(this.gameObject);
		}
	}
}
