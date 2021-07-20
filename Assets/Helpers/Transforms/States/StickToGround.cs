using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.Movement.com
{


    public class StickToGround : ITick
    {
        public bool Stick;
        Collider collider;
        StickVars vars;
        public StickToGround(Collider collider, StickVars vars)
        {
            this.collider = collider;
            this.vars = vars;
            AddTicker();
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
            Stick = MovementPrimary.SimpleStickToCollider(collider, vars);
        }


    }
}