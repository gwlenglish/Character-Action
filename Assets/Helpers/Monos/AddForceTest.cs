using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
using GWLPXL.Movement.RB.com;

/// <summary>
/// example for movement primary addforce
/// </summary>
public class AddForceTest : MonoBehaviour
{
    public KeyCode TriggerKey = KeyCode.Space;
    public AddForceVars Vars;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(TriggerKey))
        {
            MovementPrimary.AddForce(GetComponent<Rigidbody>(), Vars);
        }
    }
}
