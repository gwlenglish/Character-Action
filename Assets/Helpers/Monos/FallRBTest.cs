using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
using GWLPXL.Movement.RB.com;

public class FallRBTest : MonoBehaviour
{
    public FallingVariablesRB Vars;
    // Start is called before the first frame update
    void Start()
    {
        MovementPrimary.Falling(GetComponent<Rigidbody>(), GetComponent<Collider>(), Vars);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
