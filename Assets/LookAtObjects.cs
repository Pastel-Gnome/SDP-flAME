using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtObjects : MonoBehaviour
{
    [SerializeField] float maxLookRange;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    private MultiAimConstraint multiAimConstraint;
    [SerializeField] private RigBuilder rigBuilder;
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
            data.SetWeight(i,  Mathf.Max(maxLookRange - Vector3.Distance(playerBehaviour.rb.transform.position, LightManager.i.lightSources[i].transform.position), 0));
        }
        multiAimConstraint.data.sourceObjects = data;

        if(Vector3.Distance(lastBachPosition, playerBehaviour.rb.transform.position) > maxLookRange * 10){
            Rebatch();
        }
    }

    private void Rebatch(){
        var data = multiAimConstraint.data.sourceObjects;
        foreach(LightSource i in LightManager.i.lightSources){
            if(data.Count > 7){
                break;
            }
            if(Vector3.Distance(playerBehaviour.rb.transform.position, i.transform.position) < maxLookRange * 10){
                data.Add(new WeightedTransform(i.transform, 1 - (Vector3.Distance(playerBehaviour.rb.transform.position, i.transform.position) * maxLookRange)));
            }
        }
        multiAimConstraint.data.sourceObjects = data;
        rigBuilder.Build();
    }
}
