using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuReturn : MonoBehaviour
{
	float escTimer = 0;

	void Update()
    {
		if (Input.GetKey(KeyCode.Escape))
		{
			escTimer += Time.deltaTime;
			if (escTimer >= 2f)
			{
				if (SceneManager.GetActiveScene().name != "Main Menu")
				{
					//StartCoroutine(SaveManager.instance.SetMenuMusic(1, 0.5f));
					escTimer = 0;
					SceneManager.LoadSceneAsync("Main Menu");
				}
			}
		} else if (Input.GetKeyUp(KeyCode.Escape))
		{
			//StartCoroutine(SaveManager.instance.SetMenuMusic(0, 0.5f));
			escTimer = 0;
		}
	}
}
