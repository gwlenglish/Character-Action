using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
/// <summary>
/// teste for input move transform
/// </summary>
public class InputMoveTest : MonoBehaviour
{
    public string Horizontal = "Horizontal";
    public string Vertical = "Vertical";
    public InputMoveVars Vars;
    // Start is called before the first frame update
    void Start()
    {
        MovementPrimary.InputMove(this.transform, Vars);
    }

    // Update is called once per frame
    void Update()
    {
        Vars.X = Input.GetAxisRaw(Horizontal);
        Vars.Z = Input.GetAxisRaw(Vertical);
    }
}
