using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
	[Header("Save Data")]
	public static SaveManager instance;
	public static int saveSlot = -99;
	public static SaveData saveData = new SaveData();

	[Header("In-Game Objects")]
	public GameObject player;
	public Transform lightParent;
	public LightSource[] lanterns;
	public Holder[] holders;
	public Transform checkpointParent;

	[Header("Main Menu Objects (Do Not Set If Not On Main Menu)")]
	public Transform newGameParent;
	public Transform continueGameParent;
	public Transform newGameConfirmScreen;

	private void Awake()
	{
		if (instance != null & instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}

		SceneManager.sceneLoaded += OnSceneLoaded;
		DontDestroyOnLoad(instance);
	}

	void OnSceneLoaded(Scene currScene, LoadSceneMode loadMode)
	{
		SetupSceneData(currScene);
	}

	public static void OpenScene()
	{
		if (LoadFromFile("torchlight" + saveSlot + ".dat", out var jsonFile))
		{
			saveData.LoadFromJson(jsonFile);

			SceneManager.LoadSceneAsync(saveData.sceneName);
		}
		else
		{
			SceneManager.LoadSceneAsync("Assets/Test - Amelia/Test1_Saving_Cutscene.unity");
		}
	}

	public static void EmptySaveSlot()
	{
		File.Delete(Path.Combine(Application.persistentDataPath, "torchlight" + saveSlot + ".dat"));
	}

	void SetupSceneData(Scene currScene)
	{
		if (currScene.path == "Assets/Scenes/Game Areas/" + currScene.name || currScene.path.Contains("Test - "))
		{
			if (saveSlot == -99) { saveSlot = 1; }

			player = FindFirstObjectByType<PlayerBehaviour>().gameObject;
			lightParent = FindFirstObjectByType<LightManager>().transform;
			lanterns = lightParent.GetChild(0).GetComponentsInChildren<LightSource>();
			holders = lightParent.GetChild(1).GetComponentsInChildren<Holder>();
			checkpointParent = GameObject.Find("Checkpoints").transform;

			Holdable[] holdables = FindObjectsByType<Holdable>(FindObjectsSortMode.None);
			foreach (Holdable h in holdables)
			{
				h.Init();
			}

			if (saveData.sceneName != null && saveData.sceneName == SceneManager.GetActiveScene().name && saveData.checkpoint != -99)
			{
				LoadJsonData();
			}
			Debug.Log("Info Loaded!");
		}
		else if (currScene.name == "Main Menu")
		{
			LoadJson_MainMenu();
		}
	}

	public static void LoadJson_MainMenu()
	{
		for (int i = 1; i <= 3; i++)
		{
			if (CheckExistence("torchlight" + i + ".dat"))
			{
				instance.newGameParent.GetChild(i).GetComponent<Image>().color = new Color(1f, 0.7075472f, 0.7075472f, 1f);
				instance.continueGameParent.GetChild(i).GetComponent<Button>().interactable = true;
			}
		}
	}

	public static void LoadJsonData()
	{
		if (LoadFromFile("torchlight" + saveSlot + ".dat", out var jsonFile))
		{
			Debug.Log("Loading Data");
			saveData.LoadFromJson(jsonFile);
			bool playerHolding = false;
			PlayerBehaviour pb = instance.player.GetComponent<PlayerBehaviour>();
			pb.shadowTimer = pb.maxShadowTime;

			if (saveData.checkpoint != -99 && saveData.sceneName == SceneManager.GetActiveScene().name)
			{
				for (int i = 0; i < instance.holders.Length; i++)
				{
					instance.holders[i].currentPower = saveData.holderData[i].currentPower;
					instance.holders[i].holding = saveData.holderData[i].holdingSomething;
				}

				for (int i = 0; i < instance.lanterns.Length; i++)
				{
					instance.lanterns[i].currentRange = saveData.lanternData[i].lightRange;
					instance.lanterns[i].transform.position = new Vector3(saveData.lanternData[i].lanternPos[0], saveData.lanternData[i].lanternPos[1], saveData.lanternData[i].lanternPos[2]);
					if (saveData.lanternData[i].holderIndex != -99)
					{
						Holder dataHolder;
						if (saveData.lanternData[i].holderIndex == -19) // if held by the player
						{
							dataHolder = instance.player.GetComponent<Holder>();
							pb.animator.SetBool("Carrying", true);
							pb.animator.Play("Arms-Carry", 1);
							pb.heldObject = instance.lanterns[i].holdable;
							playerHolding = true;
						}
						else // if held by a pedestal, holster, or otherwise not held by the player
						{
							dataHolder = instance.lightParent.GetChild(1).GetChild(saveData.lanternData[i].holderIndex).GetComponentInChildren<Holder>();
						}
						Holdable holdable = instance.lanterns[i].GetComponent<Holdable>();
						//holdable.holder = dataHolder;
						holdable.grabbed(dataHolder);
					}
					else
					{
						instance.lanterns[i].GetComponent<Holdable>().loadUnheld();
						instance.lanterns[i].transform.position = new Vector3(saveData.lanternData[i].lanternPos[0], saveData.lanternData[i].lanternPos[1], saveData.lanternData[i].lanternPos[2]);
					}
				}

				instance.player.transform.position = instance.checkpointParent.GetChild(saveData.checkpoint).position;
				instance.player.transform.rotation = saveData.playerRotation;
				if (!playerHolding)
				{
					pb.animator.SetBool("Carrying", false);
					pb.animator.Play("Arms-Idle", 1);
					pb.heldObject = null;
				}
				Debug.Log("Loaded Game from Slot " + saveSlot);
			}
			else
			{
				if (SceneManager.GetActiveScene().name != "Main Menu")
				{
					SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
				}
				else
				{
					SceneManager.LoadSceneAsync("whiteboxlevel1");
				}
			}
		}
	}

	public static void SaveJsonData(int checkpointIndex)
	{
		SaveData saveData = new SaveData();

		saveData.sceneName = SceneManager.GetActiveScene().name;
		saveData.checkpoint = checkpointIndex;

		saveData.playerRotation = instance.player.transform.rotation;
		foreach (LightSource lantern in instance.lanterns)
		{
			SaveData.LanternData tempData = new SaveData.LanternData();
			tempData.lanternPos = new List<float>() { lantern.transform.position.x, lantern.transform.position.y, lantern.transform.position.z };
			tempData.lightRange = lantern.currentRange;

			if (lantern.transform.TryGetComponent<Holdable>(out Holdable tempHoldable))
			{
				//add holder to list
				if (tempHoldable.holder != null)
				{
					if (tempHoldable.holder.name != "Player")
					{
						tempData.holderIndex = tempHoldable.holder.transform.parent.GetSiblingIndex();
					}
					else if (tempHoldable.holder.name == "Player")
					{
						tempData.holderIndex = -19; // Funny number related to a "personality numbers" website I found, where the word "player" is associated with 3+7+9 (https://www.worldnumerology.com/numerology-personality/)
					}
				}
				else
				{
					tempData.holderIndex = -99;
				}
				tempData.canBeGrabbed = tempHoldable.GetGrabAbility();
				saveData.lanternData.Add(tempData);
			}
			else
			{
				// not a holdable lantern, maybe add to eventual "puzzle elements" list?
			}
		}

		foreach (Holder holder in instance.holders)
		{
			SaveData.HolderData tempData = new SaveData.HolderData();
			tempData.holdingSomething = holder.holding;
			tempData.currentPower = holder.currentPower;
			saveData.holderData.Add(tempData);
		}

		if (WriteToFile("torchlight" + saveSlot + ".dat", saveData.SaveToJson()))
		{
			Debug.Log("Saved Game to Slot " + saveSlot);
		}
	}

	public static void PostCutsceneSave(string desScene)
	{
		SaveData saveData = new SaveData();

		saveData.sceneName = desScene;
		saveData.checkpoint = -99;

		if (WriteToFile("torchlight" + saveSlot + ".dat", saveData.SaveToJson()))
		{
			Debug.Log("Saved Game to Slot 1");
		}
	}

	static bool WriteToFile(string fileName, string saveContents)
	{
		var filePath = Path.Combine(Application.persistentDataPath, fileName);
		try { File.WriteAllText(filePath, saveContents); return true; }
		catch { return false; }
	}

	static bool LoadFromFile(string fileName, out string loadContents)
	{
		var filePath = Path.Combine(Application.persistentDataPath, fileName);
		try { loadContents = File.ReadAllText(filePath); return true; }
		catch { loadContents = "no data"; return false; }
	}

	static bool CheckExistence(string fileName)
	{
		var filePath = Path.Combine(Application.persistentDataPath, fileName);
		return (File.Exists(filePath));
	}

	public static void ChangeSaveSlot(int desSlot)
	{
		saveSlot = desSlot;
	}

	public static void EmptyAllSaveSlots()
	{
		for (int i = 1; i <= 3; i++)
		{
			ChangeSaveSlot(i);
			EmptySaveSlot();

			instance.newGameParent.GetChild(i).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
			instance.continueGameParent.GetChild(i).GetComponent<Button>().interactable = false;
		}

	}

	public static void CheckNewGame()
	{
		if(CheckExistence("torchlight" + saveSlot + ".dat"))
		{
			instance.newGameConfirmScreen.gameObject.SetActive(true);
		} else
		{
			OpenScene();
		}
	}
}
