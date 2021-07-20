using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
/// <summary>
/// test for depentrate
/// </summary>
public class DepenetrateTest : MonoBehaviour
{
    public StickVars Vars;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementPrimary.Depentrate(GetComponent<CapsuleCollider>(), Vars);
    }
}
