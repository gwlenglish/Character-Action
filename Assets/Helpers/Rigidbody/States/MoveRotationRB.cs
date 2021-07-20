using GWLPXL.Movement.com;
using UnityEngine;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.RB.com
{
    public class MoveRotationRB : IPhysicsTick
    {
        RotateRBVars vars;
        Rigidbody rb;
        int timer = 0;
        public MoveRotationRB(Rigidbody rb, RotateRBVars vars)
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
            rb.MoveRotation(vars.NewRotation);
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
}