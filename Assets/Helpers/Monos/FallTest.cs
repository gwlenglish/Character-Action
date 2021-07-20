using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
/// <summary>
/// test for falling
/// </summary>
public class FallTest : MonoBehaviour
{
    public FallingVariables Vars;
    // Start is called before the first frame update
    void Start()
    {
        MovementPrimary.Falling(this.GetComponent<Collider>(), Vars);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
