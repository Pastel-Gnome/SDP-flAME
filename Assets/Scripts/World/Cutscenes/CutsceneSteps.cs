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
	[SerializeField] string nextScene;
	[SerializeField] Transform cutscenePanel;
	[SerializeField] int stepsLeft;

	private bool sceneStarted;
	private TextMeshProUGUI cutsceneText;
	private PlayerBehaviour player;

	private void Start()
	{
		cutsceneText = cutscenePanel.Find("Cutscene Text").GetComponent<TextMeshProUGUI>();
		cutscenePanel.gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player" && !sceneStarted)
		{
			HaltPlayer(other.transform);
			cutscenePanel.gameObject.SetActive(true);
			sceneStarted = true;
			NextStep();
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
		if(sceneStarted && stepsLeft > 0)
		{
			cutsceneText.text = "Number of steps left: " + stepsLeft;
			stepsLeft--;
		} else if (stepsLeft <= 0) 
		{
			EndCutscene();
		}
	}

	private void EndCutscene()
	{
		sceneStarted = false;
		SaveManager.PostCutsceneSave(nextScene);
		SceneManager.LoadSceneAsync(nextScene);
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
