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
	public Toggle mouseInvertXToggle;
	public Toggle mouseInvertYToggle;
	public TMP_Dropdown darknessTypeDropdown;

	[Header("Audio Options Objects")]
	public Slider masterSlider;
	//public Slider musicSlider;
	public Slider sfxSlider;
	public Slider ambientSlider;

	bool windowed = true;
	bool mouseInvertX = false;
	bool mouseInvertY = false;
	int darknessType = 0; // 0 for default, 1 for minimal, 2 for bright

	int resolutionChoice = 0;
	int[] resolution = { 1920, 1080 };

	float masterVolume = 0.7f;
	//float musicVolume = 1f;
	float sfxVolume = 1f;
	float ambientVolume = 1f;

	private void Start()
	{
		GetOptionValues();
		GameSetup();
	}

	public void GetOptionValues()
	{
		GetOptionPrefs();

		windowedToggle.isOn = windowed;
		mouseInvertXToggle.isOn = mouseInvertX;
		mouseInvertYToggle.isOn = mouseInvertY;

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
		SaveManager.instance.mouseInvertX = mouseInvertX;
		SaveManager.instance.mouseInvertY = !mouseInvertY;
	}

	public void SetOptionValues()
	{
		windowed = windowedToggle.isOn;
		mouseInvertX = mouseInvertXToggle.isOn;
		mouseInvertY = mouseInvertYToggle.isOn;

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
		PlayerPrefs.SetInt("invertMouseX", mouseInvertX ? 1 : 0);
		PlayerPrefs.SetInt("invertMouseY", mouseInvertY ? 1 : 0);
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
		int _windowInt = 0;
		_windowInt = PlayerPrefs.GetInt("windowed", 1);
		windowed = (_windowInt == 1);

		int _invertXInt = 0;
		_invertXInt = PlayerPrefs.GetInt("invertMouseX", 0);
		mouseInvertX = (_invertXInt == 1);
		int _invertYInt = 0;
		_invertYInt = PlayerPrefs.GetInt("invertMouseY", 0);
		mouseInvertY = (_invertYInt == 1);
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
		GameSetup();
	}

	public void SetResolution()
	{
		int w = resolution[0];
		int h = resolution[1];
		Screen.SetResolution(resolution[0], resolution[1], !windowed);
	}
}
