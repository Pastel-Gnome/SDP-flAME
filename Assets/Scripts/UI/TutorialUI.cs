using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public GameObject pressIndicator;
    public TMP_Text pressText;

    public void ShowIndicator()
    {
        pressIndicator.SetActive(true);
    }

    public void StartJumpText()
    {
        ShowIndicator();
        pressText.text = "[SPACE]";
        StartCoroutine(JumpListener());
    }

    public void StartInteractText()
    {
        ShowIndicator();
        pressText.text = "[E]";
		StartCoroutine(InteractListener());
	}

    public void StartWalkText()
    {
        ShowIndicator();
        pressText.text = "[WASD]";
		StartCoroutine(WalkListener());
	}

    private IEnumerator JumpListener()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        pressIndicator.SetActive(false);
    }

    private IEnumerator InteractListener()
    {
        while (!Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.RightShift))
        {
            yield return null;
        }
        pressIndicator.SetActive(false);
    }

	private IEnumerator WalkListener()
	{
		while (!Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.D))
		{
			yield return null;
		}
		pressIndicator.SetActive(false);
	}
}
