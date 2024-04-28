using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerOptions : MonoBehaviour
{
	[Header("Video Options Objects")]
	public Toggle windowedToggle;
	public TMP_Dropdown aspectRatioDropdown;

	[Header("Game Options Objects")]
	public TMP_Dropdown darknessTypeDropdown;

	[Header("Audio Options Objects")]
	public Slider masterSlider;
	//public Slider musicSlider;
	public Slider sfxSlider;
	public Slider ambientSlider;

	bool windowed = true;
	int darknessType = 0; // 0 for default, 1 for minimal, 2 for bright

	int aspectChoice = 0;
	int[] aspectRatio = {16, 9};

	float masterVolume = 0.7f;
	//float musicVolume = 1f;
	float sfxVolume = 1f;
	float ambientVolume = 1f;

	SaveManager saveManager;

	private void Start()
	{
		saveManager = FindFirstObjectByType<SaveManager>();
		GetOptionValues();
		GameSetup();
	}

	public void GetOptionValues()
	{
		GetOptionPrefs();

		windowedToggle.isOn = windowed;

		darknessTypeDropdown.value = darknessType;

		aspectRatioDropdown.value = aspectChoice;

		masterSlider.value = masterVolume;
		//musicSlider.value = musicVolume;
		sfxSlider.value = sfxVolume;
		ambientSlider.value = ambientVolume;
	}

	private void GameSetup()
	{
		int w = aspectRatio[0];
		int h = aspectRatio[1];
		if ((((float)Screen.width) / ((float)Screen.height)) > w / h)
		{
			Screen.SetResolution((int)(((float)Screen.height) * (w / h)), Screen.height, !windowed);
		}
		else
		{
			Screen.SetResolution(Screen.width, (int)(((float)Screen.width) * (h / w)), !windowed);
		}

	}

	public void SetOptionValues()
	{
		windowed = windowedToggle.isOn;
		darknessType = darknessTypeDropdown.value;

		aspectChoice = aspectRatioDropdown.value;
		string[] tempAspectRatio = aspectRatioDropdown.captionText.text.Split(":");
		if (int.TryParse(tempAspectRatio[0], out int i)){
			aspectRatio[0] = i;
		}
		if (int.TryParse(tempAspectRatio[1], out int j))
		{
			aspectRatio[1] = j;
		}

		masterVolume = masterSlider.value;
		//musicVolume = musicSlider.value;
		sfxVolume = sfxSlider.value;
		ambientVolume = ambientSlider.value;

		SetOptionPrefs();
	}

	private void SetOptionPrefs()
	{
		PlayerPrefs.SetInt("windowed", windowed ? 1 : 0);
		PlayerPrefs.SetInt("darknessType", darknessType);

		PlayerPrefs.SetInt("aspectChoice", aspectChoice);
		PlayerPrefs.SetInt("aspectX", aspectRatio[0]);
		PlayerPrefs.SetInt("aspectY", aspectRatio[1]);

		PlayerPrefs.SetFloat("masterVol", masterVolume);
		//PlayerPrefs.SetFloat("musicVol", musicVolume);
		PlayerPrefs.SetFloat("sfxVol", sfxVolume);
		PlayerPrefs.SetFloat("ambientVol", ambientVolume);
	}

	private void GetOptionPrefs()
	{
		int tempInt = 0;
		tempInt = PlayerPrefs.GetInt("windowed", 1);
		windowed = (tempInt == 1);

		darknessType = PlayerPrefs.GetInt("darknessType", 0);

		aspectChoice = PlayerPrefs.GetInt("aspectChoice", 0);
		aspectRatio[0] = PlayerPrefs.GetInt("aspectX", 16);
		aspectRatio[1] = PlayerPrefs.GetInt("aspectY", 9);

		masterVolume = PlayerPrefs.GetFloat("masterVol", 0.7f);
		//musicVolume = PlayerPrefs.GetFloat("musicVol", 1f);
		sfxVolume = PlayerPrefs.GetFloat("sfxVol", 1f);
		ambientVolume = PlayerPrefs.GetFloat("ambientVol", 1f);
	}

	public void SaveOptionPrefs()
	{
		PlayerPrefs.Save();
	}
}
