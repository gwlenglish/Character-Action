using UnityEngine;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.com
{
    public class SetRotationRB : IPhysicsTick
    {
        RotateRBVars vars;
        Rigidbody rb;
        int timer = 0;
        public SetRotationRB(Rigidbody rb, RotateRBVars vars)
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
            rb.rotation = vars.NewRotation;
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

    public struct RotateRBVars
    {
        public Quaternion NewRotation;
        public int Ticks;
        public RotateRBVars(Quaternion rot, int ticks = 1)
        {
            NewRotation = rot;
            Ticks = ticks;
        }
    }
}