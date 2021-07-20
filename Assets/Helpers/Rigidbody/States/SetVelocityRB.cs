using UnityEngine;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.RB.com
{
    public class SetVelocityRB : IPhysicsTick
    {
        Rigidbody rb;
        Vector3 newvelocity;
        int ticks;
        int ticker;
        public SetVelocityRB(Rigidbody rb, Vector3 newvelocity, int ticks = 1)
        {
            this.rb = rb;
            this.newvelocity = newvelocity;
            this.ticks = ticks;
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

            rb.velocity = newvelocity;
            ticker++;
            if (ticker >= ticks)
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
    public struct VelocityVars
    {
        public Vector3 Direction;
        public float Speed;
        public int Ticks;
        public VelocityVars(Vector3 direction, float speed, int ticks = 1)
        {
            Direction = direction;
            Speed = speed;
            Ticks = ticks;
        }
    }

}