using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
using GWLPXL.Movement.RB.com;

public class VelocityTest : MonoBehaviour
{
    public KeyCode TriggerKey = KeyCode.Space;
    public Vector3 NewVelocity;
    public int Ticks = 1;
    public VelocityVars Vars;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(TriggerKey))
        {
            MovementPrimary.SetVelocity(GetComponent<Rigidbody>(), NewVelocity, Ticks);
        }
    }
}
