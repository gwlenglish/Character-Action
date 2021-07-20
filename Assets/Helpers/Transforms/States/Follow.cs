using GWLPXL.Movement.com;
using UnityEngine;

namespace GWLPXL.Movement.Character.com
{
    

    public class Follow : ITick
    {
        Transform follower;
        Transform toFollow;
        FollowVars vars;
        public Follow(Transform follower, Transform toFollow, FollowVars vars)
        {
            this.follower = follower;
            this.toFollow = toFollow;
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
            follower.transform.position = Vector3.MoveTowards(follower.transform.position, toFollow.transform.position + toFollow.transform.TransformDirection(vars.Offset), vars.Speed * Time.deltaTime);

        }
    }
    [System.Serializable]
    public class FollowVars
    {
        public Vector3 Offset;
        public float Speed;
    }

}