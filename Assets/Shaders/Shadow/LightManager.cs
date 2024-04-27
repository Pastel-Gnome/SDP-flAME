using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class LightManager : MonoBehaviour
{
    public static LightManager i;
    [SerializeField] private bool enableLights = true;
    Material shadowMaterial;
    public List<LightSource> lightSources = new();
    [SerializeField] private Transform[] chargeSources;
    private Vector4[] positions = new Vector4[100];
    private float[] ranges = new float[100];

    private void Awake(){
        if(i == null){
            i = this;
        }
        else{
            Destroy(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
        for (int i = 0; i < lightSources.Count; i++)
        {
            if(Application.isPlaying){
                lightSources[i].Charge(chargeSources);
            }
            
            if(enableLights){
                positions[i] = lightSources[i].transform.position;    
                ranges[i] = lightSources[i].currentRange;    
            }           
        }

        if(!enableLights){
            Shader.SetGlobalFloat("_PlayerShadowLevel", 0);
            ranges[0] = 1000;
        }
        
        Shader.SetGlobalFloatArray("_Ranges", ranges);
        Shader.SetGlobalVectorArray("_Positions", positions);
        Shader.SetGlobalFloat("_PositionArray", lightSources.Count);
    }

    public float CalculateLightAtPoint(Vector3 targetPosition){
        float finalBrightness = 10;

        for(int i = 0; i < positions.Length; i++){
            float newBrightness = Vector3.Distance(targetPosition, positions[i]) - ranges[i];
            if(finalBrightness > newBrightness){
                finalBrightness = newBrightness;
            }
        }
        return finalBrightness;
    }
}
