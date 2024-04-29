using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneSteps : MonoBehaviour
{
	[SerializeField] Transform standSpot;
	[SerializeField] string nextActiveScene;
	[SerializeField] Transform cutscenePanel;

	[SerializeField] int thisCutsceneID;
	[SerializeField] GameObject Actor1;
	[SerializeField] GameObject Actor2;

	private bool sceneStarted;
	private int currStep;
	private DialogueController DiaControl;
	private MultiDialogueData dialogueData;
	private Image fadeoutObj;
	private PlayerBehaviour player;
	private bool playerDetected = false;

	private void Start()
	{
		DiaControl = cutscenePanel.parent.GetComponent<DialogueController>();
		dialogueData = SaveManager.instance.GetComponent<MultiDialogueData>();
		fadeoutObj = cutscenePanel.transform.parent.GetChild(1).GetComponent<Image>();

		cutscenePanel.gameObject.SetActive(false);
		StartCoroutine(FadeIn());
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player" && !sceneStarted && !playerDetected)
		{
			playerDetected = true;
			currStep = 0;
			StartCoroutine(FadeOut());
			HaltPlayer(other.transform);
		}
	}

	private void Update()
	{
		if (sceneStarted && Input.GetButtonDown("Grab"))
		{
			currStep++;
			NextStep();
		}
	}

	public void NextStep()
	{
		if(cutscenePanel.gameObject.activeSelf)
		{
			if(thisCutsceneID == 1)
			{
				if(currStep != 6) // require user to interact before moving off of Leocadia "exit" text
				{
					if (currStep == 5) // with dialogue
					{
						StartCoroutine(FadeOut());
					}
					DiaControl.DisplayNextSentenceMultiCharacters();
					if (!cutscenePanel.gameObject.activeSelf)
					{
						StartCoroutine(FadeOut());
					}
				}
			} else
			{
				DiaControl.DisplayNextSentenceMultiCharacters();
				if (!cutscenePanel.gameObject.activeSelf)
				{
					StartCoroutine(FadeOut());
				}
			}
			
		}
	}

	private void EndCutscene()
	{
		sceneStarted = false;
		playerDetected = false;
		SaveManager.PostCutsceneSave(nextActiveScene);
		SceneManager.LoadSceneAsync(nextActiveScene);
	}

	private IEnumerator FadeOut()
	{
		int tempStep = 0;
		float timer = 0;
		float maxDuration = 1;
		if(currStep == 0)
		{
			maxDuration = maxDuration / 2;
		}
		sceneStarted = false;
		while (tempStep < 4)
        {
            if(tempStep == 0)
			{
				timer += Time.fixedDeltaTime;
				float opacity = Mathf.Lerp(0, 1, timer/maxDuration);
				Color fadeColor = Color.black;
				fadeColor.a = opacity;
				fadeoutObj.color = fadeColor;
				if(opacity >= 1)
				{
					timer = 0;
					tempStep++;
				}
			} else if (tempStep == 1)
			{
				if (currStep > 0)
				{
					if (cutscenePanel.gameObject.activeSelf)
					{
						if (thisCutsceneID == 1)
						{
							Actor1.SetActive(false);
						}
					}
					else
					{
						EndCutscene();
					}
				}
				tempStep++;
			} else if (tempStep == 2)
			{
				timer += Time.fixedDeltaTime;
				float opacity = Mathf.Lerp(1, 0, timer / maxDuration);
				Color fadeColor = Color.black;
				fadeColor.a = opacity;
				fadeoutObj.color = fadeColor;
				if (opacity <= 0)
				{
					tempStep++;
				}
			} else if (tempStep == 3)
			{
				sceneStarted = true;
				if (currStep != 0)
				{
					currStep++;
					NextStep();
				} else
				{
					cutscenePanel.gameObject.SetActive(true);
					sceneStarted = true;
					DiaControl.StartMultiDialogue(dialogueData, thisCutsceneID);
				}
				tempStep++;
			}
			yield return new WaitForFixedUpdate();
		}
	}

	private IEnumerator FadeIn()
	{
		int tempStep = 0;
		float timer = 0;
		float maxDuration = 1;
		while (tempStep < 1)
		{
			if (tempStep == 0)
			{
				timer += Time.fixedDeltaTime;
				float opacity = Mathf.Lerp(1, 0, timer / maxDuration);
				Color fadeColor = Color.black;
				fadeColor.a = opacity;
				fadeoutObj.color = fadeColor;
				if (opacity <= 0)
				{
					//Debug.Log("Worked");
					timer = 0;
					tempStep++;
				}
			}
			yield return new WaitForFixedUpdate();
		}
	}

	private void HaltPlayer(Transform other)
	{
		other.position = new Vector3(standSpot.position.x, other.position.y, standSpot.position.z);
		other.rotation = standSpot.rotation;
		other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
		player = other.GetComponent<PlayerBehaviour>();
		player.isInCutscene = true;
		player.SetPlayerState(new State_Stand(player));
	}
}
