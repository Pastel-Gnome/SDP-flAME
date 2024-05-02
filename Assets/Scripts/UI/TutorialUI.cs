using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Image bgGradient, indicator;
    public GameObject container;
    public TMP_Text pressText, storyText;
    PlayerBehaviour playerRef;

    private void Start() {
        playerRef = SaveManager.instance.player.GetComponent<PlayerBehaviour>();
    }

    public void StartIntroText(){
        StartCoroutine(Fade(5f, 1, true));
        StartCoroutine(FadeStory(0.5f, 1));
        playerRef.intro = true;
        pressText.text = " Press Any Key ";
        StartCoroutine(anyKeyListener());
    }

    public void StartJumpText()
    {
        StartCoroutine(Fade(0.5f, 1, true));
        pressText.text = "[SPACE]";
        StartCoroutine(JumpListener());
    }

    public void StartInteractText()
    {
        StartCoroutine(Fade(0.5f, 1, true));
        pressText.text = "[E]";
		StartCoroutine(InteractListener());
	}

    public void StartWalkText()
    {
        StartCoroutine(Fade(0.5f, 1, true));
        pressText.text = "[WASD]";
		StartCoroutine(WalkListener());
	}

    private IEnumerator anyKeyListener()
    {
        float timeElapsed = 0;
        while (!Input.anyKeyDown || timeElapsed < 4)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(FadeStory(0.5f, 0));
        StartWalkText();
        
        playerRef.intro = false;
        playerRef.introShadow = 2.25f;
    }

    private IEnumerator JumpListener()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        StartCoroutine(Fade(0.25f, 0, false));
    }

    private IEnumerator InteractListener()
    {
        while (!Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.RightShift))
        {
            yield return null;
        }
        StartCoroutine(Fade(0.25f, 0, false));
    }

	private IEnumerator WalkListener()
	{
		while (!Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.D))
		{
			yield return null;
		}
		StartCoroutine(Fade(0.25f, 0, false));
	}

    private IEnumerator Fade(float duration, float targetOpacity, bool finishState){
        float timeElapsed = 0;
        float startOpacity = bgGradient.color.a;
        if(finishState){
            container.SetActive(true);
        }
        while (timeElapsed < duration){
            timeElapsed += Time.deltaTime;
            float currentOpacity = Mathf.Lerp(startOpacity, targetOpacity, Mathf.Min(timeElapsed- (duration-1), 1)/1);
            bgGradient.color = new Color(bgGradient.color.r, bgGradient.color.g, bgGradient.color.b, currentOpacity);
            indicator.color = new Color(indicator.color.r, indicator.color.g, indicator.color.b, currentOpacity);
            pressText.color = new Color(pressText.color.r, pressText.color.g, pressText.color.b, currentOpacity);
            yield return new WaitForEndOfFrame();
        }
        if(!finishState){
            container.SetActive(false);
        }
    }

    private IEnumerator FadeStory(float duration, float targetOpacity){
        float timeElapsed = 0;
        float startOpacity = storyText.color.a;
        
        while (timeElapsed < duration){
            timeElapsed += Time.deltaTime;
            float currentOpacity = Mathf.Lerp(startOpacity, targetOpacity, Mathf.Min(timeElapsed-(duration-1), 1)/1);
            storyText.color = new Color(storyText.color.r, storyText.color.g, storyText.color.b, currentOpacity);
            yield return new WaitForEndOfFrame();
        }
    }
}
