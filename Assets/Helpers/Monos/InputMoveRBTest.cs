using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
using GWLPXL.Movement.RB.com;

/// <summary>
/// test for input move rigidbody
/// </summary>
public class InputMoveRBTest : MonoBehaviour
{
    public string Horizontal = "Horizontal";
    public string Vertical = "Vertical";
    public InputMoveVarsRB Vars;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InputMoveVarsRB newvars = new InputMoveVarsRB(

            Input.GetAxisRaw(Horizontal),
            0,
            Input.GetAxisRaw(Vertical),
            Vars.Force,
            Vars.Type,
            Vars.ForceMode,
            Vars.Reference
            );
        
        MovementPrimary.InputMove(GetComponent<Rigidbody>(), newvars);

    }
}
