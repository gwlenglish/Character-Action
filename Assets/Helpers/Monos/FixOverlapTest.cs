using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;

/// <summary>
/// test for fix overlapped colliders
/// </summary>
public class FixOverlapTest : MonoBehaviour
{
    public LayerMask LayerMask;
    public float FixSpeed = 55;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementPrimary.TryFixOverlap(GetComponent<CapsuleCollider>(), LayerMask, new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")), FixSpeed);
    }
}
