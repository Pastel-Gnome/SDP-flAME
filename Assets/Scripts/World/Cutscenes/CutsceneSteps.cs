using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CutsceneSteps : MonoBehaviour
{
	[SerializeField] Transform standSpot;
	[SerializeField] string nextActiveScene;
	[SerializeField] Transform cutscenePanel;

	[SerializeField] int thisCutsceneID;

	private bool sceneStarted;
	private DialogueController DiaControl;
	private MultiDialogueData dialogueData;
	private PlayerBehaviour player;

	private void Start()
	{
		DiaControl = cutscenePanel.parent.GetComponent<DialogueController>();
		dialogueData = FindFirstObjectByType<SaveManager>().GetComponent<MultiDialogueData>();

		cutscenePanel.gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player" && !sceneStarted)
		{
			HaltPlayer(other.transform);
			cutscenePanel.gameObject.SetActive(true);
			sceneStarted = true;
			DiaControl.StartMultiDialogue(dialogueData, thisCutsceneID);
		}
	}

	private void Update()
	{
		if (sceneStarted && Input.GetButtonDown("Grab"))
		{
			NextStep();
		}
	}

	public void NextStep()
	{
		if(cutscenePanel.gameObject.activeSelf)
		{
			DiaControl.DisplayNextSentenceMultiCharacters();
			if (!cutscenePanel.gameObject.activeSelf)
			{
				EndCutscene();
			}
		}
	}

	private void EndCutscene()
	{
		sceneStarted = false;
		SaveManager.PostCutsceneSave(nextActiveScene);
		SceneManager.LoadSceneAsync(nextActiveScene);
	}

	private void HaltPlayer(Transform other)
	{
		other.position = new Vector3(standSpot.position.x, other.position.y, standSpot.position.z);
		other.rotation = standSpot.rotation;
		other.GetComponent<Rigidbody>().isKinematic = true;
		player = other.GetComponent<PlayerBehaviour>();
		player.isInCutscene = true;
		player.SetPlayerState(new State_Stand(player));
	}
}
