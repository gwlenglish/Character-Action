using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
using GWLPXL.Movement.RB.com;

/// <summary>
/// test for input rotate RB
/// </summary>
public class InputRotateRBTest : MonoBehaviour
{
    public string Horizontal = "Horizontal";
    public string Vertical = "Vertical";
    public InputRotateVarsRB Vars;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputRotateVarsRB newvars = new InputRotateVarsRB(

          Input.GetAxisRaw(Horizontal),
          0,
          Input.GetAxisRaw(Vertical),
          Vars.Speed,
          Vars.Type,
          Vars.Smooth,
          Vars.Reference
          );

        MovementPrimary.InputRotateRB(GetComponent<Rigidbody>(), newvars);
    }
}
