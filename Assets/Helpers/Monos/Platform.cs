using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
using GWLPXL.Movement.RB.com;

/// <summary>
/// needs to happen in order of movement, make a new movement type for platforms.
/// </summary>
public class Platform : MonoBehaviour
{
    public MoveLerpVarsRB Vars;
    Rigidbody rb;
    MoveLerpRB move;
    // Start is called before the first frame update
    void Start()
    {
        move = MovementPrimary.MoveLerp(GetComponent<Rigidbody>(), Vars, true);
        // Movement.MoveLerp(GetComponent<Rigidbody>(), Vars, true, Time.deltaTime, Time.fixedDeltaTime);

    }


    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //ride
            move.AddRider(rb);

        }

        CharacterController charcon = other.GetComponent<CharacterController>();
        if (charcon != null)
        {
            move.AddRider(charcon);
           
        }
    }

  
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //ride
            move.RemoveRider(rb);

        }

        CharacterController charcon = other.GetComponent<CharacterController>();
        if (charcon != null)
        {
            move.RemoveRider(charcon);

        }
       
    }
 

  

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }



}



