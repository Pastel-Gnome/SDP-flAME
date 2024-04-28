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
    public float ambienceVolume;
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
        audioMixer.SetFloat("MasterVolume", 5.47f  + (80 * (masterVolume - 1)));
        audioMixer.SetFloat("AmbientVolume", -0.06f + (80 * (ambienceVolume - 1)));
		audioMixer.SetFloat("MusicVolume", -5.24f + (80 * (ambienceVolume - 1)));
		audioMixer.SetFloat("SFXVolume", -9.93f + (80 * (sfxVolume - 1)));
        
        dangerAudioBaseLevel = dangerAudio.volume;
        dangerAudio.Play();
        caveAudio.Play();
    }

    public void SetDangerLevel(float dangerLevel){
        //print("wagoogie");
        dangerAudio.volume = Mathf.Lerp(0, dangerAudioBaseLevel, dangerLevel);
    }
}
