using UnityEngine;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.com
{
    public class SetRotation : ITick
    {
        Transform transform;
        Quaternion rot;
        int timer = 0;
        public SetRotation(Transform rb, Quaternion rot)
        {
            this.transform = rb;
            this.rot = rot;
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
            transform.rotation = rot;
            RemoveTicker();

        }
    }
}