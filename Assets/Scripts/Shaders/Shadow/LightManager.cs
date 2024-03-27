using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class LightManager : MonoBehaviour
{
    [SerializeField] private bool enableLights = true;
    Material shadowMaterial;
    [SerializeField] private LightSource[] lightSources;
    [SerializeField] private Transform[] chargeSources;
    private Vector4[] positions = new Vector4[100];
    private float[] ranges = new float[100];

    // Start is called before the first frame update
    void Start()
    {
        lightSources = GetComponentsInChildren<LightSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < lightSources.Length; i++)
        {
            lightSources[i].Charge(chargeSources);
            
            if(enableLights){
                positions[i] = lightSources[i].transform.position;    
                ranges[i] = lightSources[i].currentRange;    
            }           
        }

        if(!enableLights){
            ranges[0] = 1000;
        }

        Shader.SetGlobalFloatArray("_Ranges", ranges);
        Shader.SetGlobalVectorArray("_Positions", positions);
        Shader.SetGlobalFloat("_PositionArray", lightSources.Length);
    }
}
