using UnityEngine;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.RB.com
{
    public enum RigidbodyMoveType
    {
        MovePosition = 1,
        Velocity = 2,
        AddForce = 3
    }
    public enum RigibodyMoveType
    {
        AddForce = 0,
        AddVelocity = 1,
        SetVelocity = 2,
        MovePosition = 3
    }
    public enum RigibodyRotateType
    {
        MoveRotation = 0,
        SetRotation = 1,
    }
    public class MovePositionRB : IPhysicsTick
    {
        MovePositionVars vars;
        Rigidbody rb;
        int timer = 0;
        public MovePositionRB(Rigidbody rb, MovePositionVars vars)
        {
            this.rb = rb;
            this.vars = vars;
            AddTicker();
        }
        public void AddTicker()
        {
            TickManager.AddTicker(this);
        }

        public float GetTickDuration()
        {
            return Time.fixedDeltaTime;
        }

        public void PhysicsTick()
        {
            if (rb == null)
            {
                RemoveTicker();
                return;
            }
            rb.MovePosition(vars.NewPosition);
            timer++;
            if (timer >= vars.Ticks)
            {
                RemoveTicker();
            }

        }

        public void RemoveTicker()
        {
            TickManager.RemoveTicker(this);
        }
    }

    [System.Serializable]
    public struct MovePositionVars
    {
        public Vector3 NewPosition;
        public int Ticks;
        public MovePositionVars(Vector3 newposition, int ticks = 1)
        {
            NewPosition = newposition;
            Ticks = ticks;
        }
    }

}