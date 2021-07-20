using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
public class TransformTest : MonoBehaviour
{
    public KeyCode KeyToTrigger = KeyCode.Alpha1;
    public SimpleMoveVars Vars;
    bool allowmove = false;
    // Start is called before the first frame update
    void Start()
    {
        MovementPrimary.SimpleMove(this.transform, Vars);
       
        allowmove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyToTrigger) && allowmove)
        {
            allowmove = false;
            MovementPrimary.SimpleMove(this.transform, Vars, AllowMove);
        }
    }

  
    void AllowMove(SimpleMove move)
    {
        allowmove = true;
        move.OnComplete -= AllowMove;

    }
}
