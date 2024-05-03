using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager i;
    public AudioSource caveAudio;
    public AudioSource dangerAudio;
    public AudioMixer audioMixer;
    public float masterVolume;
    public float ambientVolume;
    public float musicVolume;
    public float sfxVolume;
	private float dangerAudioBaseLevel;
    private void Awake() {
        if(!i){
            i = this;
        }
        else{
            Destroy(this);
        }
    }

    private void Start()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log(masterVolume) * 20);
        audioMixer.SetFloat("AmbientVolume", Mathf.Log(ambientVolume) * 20);
		audioMixer.SetFloat("MusicVolume", Mathf.Log(musicVolume) * 20);
		audioMixer.SetFloat("SFXVolume", Mathf.Log(sfxVolume) * 20);
        
        dangerAudioBaseLevel = dangerAudio.volume;
        dangerAudio.Play();
        caveAudio.Play();
    }

    public void SetDangerLevel(float dangerLevel){
        //print("wagoogie");
        dangerAudio.volume = Mathf.Lerp(0, dangerAudioBaseLevel, dangerLevel);
    }
}
