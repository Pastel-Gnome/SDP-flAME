using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanism : MonoBehaviour
{
    [SerializeField] private PowerSource[] powerSources;
    [SerializeField] private MechanismAction[] mechanismActions;
    [SerializeField] private float maxPowerInput;
    [SerializeField] private AudioClip powerHumNoise;
    private float powerHumVolume;
	private AudioSource powerHumAudioSource;
    public float currentPowerInput;

    // Start is called before the first frame update
    private void Start()
    {
        powerHumAudioSource = GetComponent<AudioSource>();
        if(powerHumNoise){
            powerHumVolume = powerHumAudioSource.volume;
            powerHumAudioSource.clip = powerHumNoise;
            powerHumAudioSource.Play();
        }
        
        foreach(MechanismAction i in  mechanismActions){
            i.OnStart(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        powerHumAudioSource.volume = Mathf.Lerp(powerHumAudioSource.volume, Mathf.Lerp(0, powerHumVolume, currentPowerInput), 0.1f);
        powerHumAudioSource.pitch = Mathf.Lerp(powerHumAudioSource.pitch, Mathf.Lerp(-5, 1, currentPowerInput), 0.1f);
        foreach(MechanismAction i in  mechanismActions){
            i.OnUpdate(this);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float totalInput = 0;
        foreach(PowerSource i in powerSources){totalInput += i.currentPower;}
        totalInput = totalInput + 0.1f > maxPowerInput ? maxPowerInput : totalInput;
        totalInput = totalInput - 0.1f < 0 ? 0 : totalInput;
        currentPowerInput = totalInput;

        foreach(MechanismAction i in mechanismActions){
            i.OnFixedUpdate(this);
        }
    }

    private void Activate(){

    }

    private void Deactivate(){

    }
}
