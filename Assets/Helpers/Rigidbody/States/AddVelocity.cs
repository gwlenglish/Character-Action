using UnityEngine;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.RB.com
{
    public class AddVelocity : IPhysicsTick
    {
        VelocityVars vars;
        Rigidbody rb;
        int timer = 0;
        public AddVelocity(Rigidbody rb, VelocityVars vars)
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
            float dt = GetTickDuration();
            rb.velocity += vars.Direction * vars.Speed * dt;
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