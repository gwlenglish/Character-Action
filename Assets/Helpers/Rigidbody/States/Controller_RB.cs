using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
using GWLPXL.Movement.RB.com;

[System.Serializable]
public class StandardMovementRB
{
    public InputRotateVarsRB Rotation;
    public InputMoveVarsRB Movement;
    public FallingVariablesRB Falling;

}
public class Controller_RB : MonoBehaviour, ITick
{
    public StandardMovementRB Standard;
    string horizontal = "Horizontal";
    string vertical = "Vertical";

    private void Start()
    {
        AddTicker();
        MovementPrimary.Falling(GetComponent<Rigidbody>(), GetComponent<Collider>(), Standard.Falling);
    }

    private void OnDestroy()
    {
        RemoveTicker();
    }
    public void AddTicker()
    {
        TickManager.AddTicker(this);
    }

    public float GetTickDuration()
    {
        return Time.deltaTime;
    }

    public void RemoveTicker()
    {
        TickManager.RemoveTicker(this);
    }

    public void Tick()
    {
        Standard.Movement.X = Input.GetAxisRaw(horizontal);
        Standard.Movement.Z = Input.GetAxisRaw(vertical);
        Standard.Rotation.X = Input.GetAxisRaw(horizontal);
        Standard.Rotation.Z = Input.GetAxisRaw(vertical);
        MovementPrimary.InputMove(GetComponent<Rigidbody>(), Standard.Movement);
        MovementPrimary.InputRotateRB(GetComponent<Rigidbody>(), Standard.Rotation);
    }

   
}
