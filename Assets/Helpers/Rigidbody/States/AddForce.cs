using UnityEngine;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.RB.com
{
    public class AddForce : IPhysicsTick
    {
        AddForceVars vars;
        Rigidbody rb;
        int timer = 0;
        public AddForce(Rigidbody rb, AddForceVars vars)
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
            rb.AddForce(vars.Direction, vars.Force);
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
    public struct AddForceVars
    {
        public Vector3 Direction;
        public ForceMode Force;
        public int Ticks;
        public AddForceVars(Vector3 dir, ForceMode force, int ticks = 1)
        {
            Direction = dir;
            Force = force;
            Ticks = ticks;
        }
    }


}