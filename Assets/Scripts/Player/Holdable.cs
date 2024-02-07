using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    public Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out Rigidbody rbNew);
        rb = rbNew;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
