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
					escTimer = 0;
					SceneManager.LoadSceneAsync("Main Menu");
				}
			}
		} else if (Input.GetKeyUp(KeyCode.Escape))
		{
			escTimer = 0;
		}
	}
}
