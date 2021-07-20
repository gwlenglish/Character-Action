using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
public class MoveTestT : MonoBehaviour
{
    public MoveLerpVars Vars;
    // Start is called before the first frame update
    void Start()
    {
        MovementPrimary.MoveLerp(this.transform, Vars, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
