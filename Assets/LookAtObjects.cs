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

    // Start is called before the first frame update
    void Start()
    {
        multiAimConstraint = GetComponent<MultiAimConstraint>();

        var data = multiAimConstraint.data.sourceObjects;

        Transform chosenTransform = null;
        float iToClosest = 1000;

        foreach(LightSource i in LightManager.i.lightSources){
            float iToPlayer = Vector3.Distance(i.transform.position, playerBehaviour.rb.transform.position);
            
            if(iToPlayer < maxLookRange && !chosenTransform || (iToPlayer < iToClosest)){
                iToClosest = iToPlayer;
                chosenTransform = i.transform;
            }
        }
        if(chosenTransform){data.Add(new WeightedTransform(chosenTransform, 1));}

        multiAimConstraint.data.sourceObjects = data;

        rigBuilder.Build();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
