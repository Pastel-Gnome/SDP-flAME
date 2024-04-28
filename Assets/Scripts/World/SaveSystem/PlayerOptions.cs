using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerOptions : MonoBehaviour
{
	[Header("Video Options Objects")]
	public Toggle windowedToggle;
	public TMP_Dropdown resolutionDropdown;

	[Header("Game Options Objects")]
	public TMP_Dropdown darknessTypeDropdown;

	[Header("Audio Options Objects")]
	public Slider masterSlider;
	//public Slider musicSlider;
	public Slider sfxSlider;
	public Slider ambientSlider;

	bool windowed = true;
	int darknessType = 0; // 0 for default, 1 for minimal, 2 for bright

	int resolutionChoice = 0;
	int[] resolution = { 1920, 1080 };

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

		resolutionDropdown.value = resolutionChoice;

		masterSlider.value = masterVolume;
		//musicSlider.value = musicVolume;
		sfxSlider.value = sfxVolume;
		ambientSlider.value = ambientVolume;
	}

	private void GameSetup()
	{
		SetResolution();

	}

	public void SetOptionValues()
	{
		windowed = windowedToggle.isOn;
		darknessType = darknessTypeDropdown.value;

		resolutionChoice = resolutionDropdown.value;
		string[] tempAspectRatio = resolutionDropdown.captionText.text.Split(" x ");
		if (int.TryParse(tempAspectRatio[0], out int i))
		{
			resolution[0] = i;
		}
		if (int.TryParse(tempAspectRatio[1], out int j))
		{
			resolution[1] = j;
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

		PlayerPrefs.SetInt("resolutionChoice", resolutionChoice);
		PlayerPrefs.SetInt("resX", resolution[0]);
		PlayerPrefs.SetInt("resY", resolution[1]);

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

		resolutionChoice = PlayerPrefs.GetInt("resolutionChoice", 0);
		resolution[0] = PlayerPrefs.GetInt("resX", 1920);
		resolution[1] = PlayerPrefs.GetInt("resY", 1080);

		masterVolume = PlayerPrefs.GetFloat("masterVol", 0.7f);
		//musicVolume = PlayerPrefs.GetFloat("musicVol", 1f);
		sfxVolume = PlayerPrefs.GetFloat("sfxVol", 1f);
		ambientVolume = PlayerPrefs.GetFloat("ambientVol", 1f);
	}

	public void SaveOptionPrefs()
	{
		PlayerPrefs.Save();
		SetResolution();
	}

	public void SetResolution()
	{
		int w = resolution[0];
		int h = resolution[1];
		Screen.SetResolution(resolution[0], resolution[1], !windowed);
	}
}
