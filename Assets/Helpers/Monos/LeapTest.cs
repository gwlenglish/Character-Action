using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
/// <summary>
/// test for leap transform
/// </summary>
public class LeapTest : MonoBehaviour
{
    public KeyCode Trigger = KeyCode.Space;
    public LeapVars Vars;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Trigger))
        {
            MovementPrimary.Leap(this.transform, Vars);//also needs to block

        }
    }
}
