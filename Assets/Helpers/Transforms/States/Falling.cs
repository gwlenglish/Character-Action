using GWLPXL.Movement.Character.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.Movement.com
{
    [System.Serializable]
    public class FallingVariables
    {
        public float Multiplier = 1;
        public LayerMask GroundLayer;
        public float FallingSpeed;
        public float CoyoteDuration = .25f;
        public AnimationCurve FallingCurve;
        public float TimeToMaxFallSpeed;
        public Vector3 GravityDirection;
        public bool IsGrounded;
        public FallingVariables(Vector3 gravitydir, LayerMask ground, float fallingspeed, float timetomaxspeed, AnimationCurve curve, float coyote = .25f, float multi = 1)
        {
            GroundLayer = ground;
            GravityDirection = gravitydir;
            FallingSpeed = fallingspeed;
            TimeToMaxFallSpeed = timetomaxspeed;
            FallingCurve = curve;
            CoyoteDuration = coyote;
            Multiplier = multi;
        }

    }

    public class Falling : ITick
    {
        FallingVariables vars;//change these to normal falling
        Collider collider;
        float timer;

        public Falling(Collider collider, FallingVariables vars)
        {
            this.vars = vars;
            this.collider = collider;
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
            CapsuleCollider capsule = collider as CapsuleCollider;
            bool grounded = Detection.SimpleSpherecast(capsule.bounds.max, capsule.radius * .9f, vars.GravityDirection, capsule.height * .9f, vars.GroundLayer);

           // bool grounded = Detection.SimpleSpherecast(collider.bounds.min, capsule.radius * .9f, vars.GravityDirection, vars.FallingSpeed * Time.deltaTime, vars.GroundLayer);
            if (grounded)
            {
                timer = 0;
                return;
            }
            timer += GetTickDuration();
            float newY = 0;
            float newz = 0;
            float newX = 0;
            float percent = timer / vars.TimeToMaxFallSpeed;
            if (vars.FallingCurve != null)
            {
                percent = vars.FallingCurve.Evaluate(percent);
            }
            newY = percent * vars.FallingSpeed * vars.Multiplier;// * GetTickDuration();


            Vector3 moveX = new Vector3(newX, 0, 0);//add control later
            Vector3 moveZ = new Vector3(0, 0, newz);//add control later
            Vector3 moveY = new Vector3(0, newY, 0);
            Vector3 additionalMove = moveX + moveY + moveZ;

            Vector3 goal = vars.GravityDirection * vars.FallingSpeed + additionalMove;
            Vector3 scaledgoal = Vector3.Scale(goal, vars.GravityDirection);
            Vector3 scaled = Vector3.Scale(collider.transform.position, vars.GravityDirection);
            Vector3 lerp = Vector3.Lerp(scaled, scaledgoal, percent);
            Vector3 direction = Vector3.Scale(collider.transform.position, vars.GravityDirection) - lerp;


            MovementPrimary.SimpleMove(collider.transform, new SimpleMoveVars(direction.normalized, newY, Time.deltaTime));




        }


    }

}