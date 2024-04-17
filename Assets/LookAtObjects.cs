using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtObjects : MonoBehaviour
{
    [SerializeField] float maxLookRange;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    private MultiAimConstraint multiAimConstraint;
    [SerializeField] private RigBuilder rigBuilder;
    private Transform[] nearbyLights;
    private Vector3 lastBachPosition;

    // Start is called before the first frame update
    private void Start()
    {
        multiAimConstraint = GetComponent<MultiAimConstraint>();
        Rebatch();
    }

    private void Update(){
        var data = multiAimConstraint.data.sourceObjects;
        for(int i = 0; i < data.Count; i++){
            data.SetWeight(i,  Mathf.Max(maxLookRange - Vector3.Distance(playerBehaviour.rb.transform.position, nearbyLights[i].transform.position), 0));
        }
        multiAimConstraint.data.sourceObjects = data;

        if(Vector3.Distance(lastBachPosition, playerBehaviour.rb.transform.position) > maxLookRange * 10){
            Rebatch();
        }
    }

    private void Rebatch(){
        var data = multiAimConstraint.data.sourceObjects;
        nearbyLights = new Transform[8];
        for(int i = 0; i < data.Count; i++){
            Transform currentLight = LightManager.i.lightSources[i].transform;
            if(data.Count > 7){
                break;
            }
            if(Vector3.Distance(playerBehaviour.rb.transform.position, currentLight.position) < maxLookRange * 10){
                data.Add(new WeightedTransform(currentLight, 1 - (Vector3.Distance(playerBehaviour.rb.transform.position, currentLight.position) * maxLookRange)));
                nearbyLights.Append(currentLight);
            }
        }
        multiAimConstraint.data.sourceObjects = data;
        rigBuilder.Build();
    }
}
