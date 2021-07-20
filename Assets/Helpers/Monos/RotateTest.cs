using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
public class RotateTest : MonoBehaviour
{
    public RotateVars Vars;
    // Start is called before the first frame update
    void Start()
    {
        MovementPrimary.Rotate(this.transform, Vars);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
