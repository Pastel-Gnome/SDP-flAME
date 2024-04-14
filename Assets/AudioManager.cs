using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager i;
    public AudioSource caveAudio;
    public AudioSource dangerAudio;
    private float dangerAudioBaseLevel;
    private void Awake() {
        if(!i){
            i = this;
        }
        else{
            Destroy(this);
        }
    }

    private void Start(){
        dangerAudioBaseLevel = dangerAudio.volume;
        dangerAudio.Play();
        caveAudio.Play();
    }

    public void SetDangerLevel(float dangerLevel){
        //print("wagoogie");
        dangerAudio.volume = Mathf.Lerp(0, dangerAudioBaseLevel, dangerLevel);
    }
}
