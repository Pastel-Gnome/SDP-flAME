using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    Material shadowMaterial;
    [SerializeField] private LightSource[] lightSources;
    [SerializeField] private Transform[] chargeSources;
    [SerializeField] private float chargeRange;
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
            positions[i] = lightSources[i].transform.position;    
            ranges[i] = lightSources[i].currentRange;                   
        }
        
        foreach(LightSource i in lightSources){
            bool foundChargeSource = false;
            if(i.holdable && i.holdable.holder){
                foundChargeSource = true;
            }
            else{
                foreach(Transform j in chargeSources){
                    if(Vector2.Distance(i.transform.position, j.position) < chargeRange){
                        foundChargeSource = true;
                        break;
                    }
                }
            }

            i.currentRange = foundChargeSource ? Mathf.Lerp(i.currentRange, i.maxRange, 0.01f) : i.currentRange -= i.decay;
            i.currentRange = i.currentRange < 0 ? 0 : i.currentRange;
        }

        //Shader.SetGlobalVectorArray("_lightSources", lightSources);
        Shader.SetGlobalFloatArray("_Ranges", ranges);
        Shader.SetGlobalVectorArray("_Positions", positions);
        Shader.SetGlobalFloat("_PositionArray", lightSources.Length);
    }
}
