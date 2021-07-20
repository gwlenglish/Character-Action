using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
/// <summary>
/// example for align to ground
/// </summary>
public class AlignToGroundTest : MonoBehaviour
{
    public StickVars Vars;
    // Start is called before the first frame update
    void Start()
    {
        MovementPrimary.StickToGround(GetComponent<Collider>(), Vars);
    }

    // Update is called once per frame
    void Update()
    {
   
    }
}
