using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LightManager : MonoBehaviour
{
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
    void FixedUpdate()
    {
        for (int i = 0; i < lightSources.Length; i++)
        {
            lightSources[i].Charge(chargeSources);
            positions[i] = lightSources[i].transform.position;    
            ranges[i] = lightSources[i].currentRange;                   
        }

        //Shader.SetGlobalVectorArray("_lightSources", lightSources);
        Shader.SetGlobalFloatArray("_Ranges", ranges);
        Shader.SetGlobalVectorArray("_Positions", positions);
        Shader.SetGlobalFloat("_PositionArray", lightSources.Length);
    }
}
