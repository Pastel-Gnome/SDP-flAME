using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerOptions : MonoBehaviour
{
	[Header("Video Options Objects")]
	public Toggle windowedToggle;
	public TMP_Dropdown resolutionDropdown;

	[Header("Game Options Objects")]
	public Toggle mouseInvertXToggle;
	public Toggle mouseInvertYToggle;

	[Header("Audio Options Objects")]
	public Slider masterSlider;
	public Slider musicSlider;
	public Slider sfxSlider;
	public Slider ambientSlider;
	public AudioMixer audioMixer;

	bool windowed = true;
	bool mouseInvertX = true;
	bool mouseInvertY = false;
	int darknessType = 0; // 0 for default, 1 for minimal, 2 for bright

	int resolutionChoice = 0;
	int[] resolution = { 1920, 1080 };

	float masterVolume = 0.7f;
	float musicVolume = 1f;
	float sfxVolume = 1f;
	float ambientVolume = 1f;

	AudioSource menuMusic;

	private void Start()
	{
		GetOptionValues();
		GameSetup();
		menuMusic = SaveManager.instance.GetComponent<AudioSource>();
		StartCoroutine(SetMenuMusic(SceneManager.GetActiveScene().name == "Main Menu" ? 1 : 0, 1f));
	}

	public void GetOptionValues()
	{
		GetOptionPrefs();

		windowedToggle.isOn = windowed;
		mouseInvertXToggle.isOn = mouseInvertX;
		mouseInvertYToggle.isOn = mouseInvertY;

		resolutionDropdown.value = resolutionChoice;

		masterSlider.value = masterVolume;
		musicSlider.value = musicVolume;
		sfxSlider.value = sfxVolume;
		ambientSlider.value = ambientVolume;

		masterSlider.value = Mathf.InverseLerp(0.0001f, 1.2f, masterVolume);
		musicSlider.value = Mathf.InverseLerp(0.0001f, 1f, musicVolume);
		sfxSlider.value = Mathf.InverseLerp(0.0001f, 1f, sfxVolume);
		ambientSlider.value = Mathf.InverseLerp(0.0001f, 1f, ambientVolume);
	}

	private void GameSetup()
	{
		SetResolution();
		SaveManager.instance.mouseInvertX = mouseInvertX;
		SaveManager.instance.mouseInvertY = !mouseInvertY;
		audioMixer.SetFloat("MasterVolume", Mathf.Log(masterVolume) * 20);
		audioMixer.SetFloat("AmbientVolume", Mathf.Log(ambientVolume) * 20);
		audioMixer.SetFloat("MusicVolume", Mathf.Log(musicVolume) * 20);
		audioMixer.SetFloat("SFXVolume", Mathf.Log(sfxVolume) * 20);
	}

	public void SetOptionValues()
	{
		windowed = windowedToggle.isOn;
		mouseInvertX = mouseInvertXToggle.isOn;
		mouseInvertY = mouseInvertYToggle.isOn;

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

		masterVolume = Mathf.Lerp(0.0001f, 1.2f, masterSlider.value);
		musicVolume = Mathf.Lerp(0.0001f, 1f, musicSlider.value);
		sfxVolume = Mathf.Lerp(0.0001f, 1f, sfxSlider.value);
		ambientVolume = Mathf.Lerp(0.0001f, 1f, ambientSlider.value);

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
		PlayerPrefs.SetFloat("musicVol", musicVolume);
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

		masterVolume = PlayerPrefs.GetFloat("masterVol", Mathf.Lerp(0.0001f, 1.2f, 0.7f));
		musicVolume = PlayerPrefs.GetFloat("musicVol", 1f);
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

	public IEnumerator SetMenuMusic(float newVolume, float duration)
	{
		float startVolume = menuMusic.volume;
		float timeElapsed = 0;
		while (timeElapsed < duration)
		{
			timeElapsed += Time.deltaTime;
			menuMusic.volume = Mathf.Lerp(startVolume, newVolume, timeElapsed / duration);
		}
		yield return new WaitForFixedUpdate();
	}
}
