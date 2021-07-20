using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
using GWLPXL.Movement.RB.com;
/// <summary>
/// teset for rb move lerp
/// </summary>
public class MoveTest : MonoBehaviour
{
    public MoveLerpVarsRB Vars;
    // Start is called before the first frame update
    void Start()
    {
          MovementPrimary.MoveLerp(GetComponent<Rigidbody>(), Vars, true);
       // Movement.MoveLerp(GetComponent<Rigidbody>(), Vars, true, Time.deltaTime, Time.fixedDeltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
