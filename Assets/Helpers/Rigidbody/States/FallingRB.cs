using GWLPXL.Movement.com;
using UnityEngine;

namespace GWLPXL.Movement.RB.com
{
    public class FallingRB : ITick
    {
        FallingVariablesRB vars;
        Collider collider;
        float timer;
        Rigidbody rigidbody;

        public FallingRB(Rigidbody rigidbody, Collider collider, FallingVariablesRB vars)
        {
            this.rigidbody = rigidbody;
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
            bool grounded = Detection.SimpleSpherecast(capsule.bounds.max, capsule.radius * .9f, vars.GravityDirection, .1f, vars.GroundLayer);

            // bool grounded = Detection.SimpleSpherecast(collider.bounds.min, capsule.radius * .9f, vars.GravityDirection, vars.FallingSpeed * Time.deltaTime, vars.GroundLayer);
            if (grounded)
            {
                timer = 0;
                return;
            }
            float dt = GetTickDuration();
            timer += dt;
            float newY = 0;
            float newz = 0;
            float newX = 0;
            float percent = timer / vars.TimeToMaxFallSpeed;
            if (vars.FallingCurve != null)
            {
                percent = vars.FallingCurve.Evaluate(percent);
            }
            newY = percent * vars.FallingForce;// * GetTickDuration();


            Vector3 moveX = new Vector3(newX, 0, 0);//add control later
            Vector3 moveZ = new Vector3(0, 0, newz);//add control later
            Vector3 moveY = new Vector3(0, newY, 0);
            Vector3 additionalMove = moveX + moveY + moveZ;

            Vector3 goal = vars.GravityDirection * vars.FallingForce + additionalMove;
            Vector3 scaledgoal = Vector3.Scale(goal, vars.GravityDirection);
            Vector3 scaled = Vector3.Scale(collider.transform.position, vars.GravityDirection);
            Vector3 lerp = Vector3.Lerp(scaled, scaledgoal, percent);
            Vector3 direction = Vector3.Scale(collider.transform.position, vars.GravityDirection) - lerp;

            MovementPrimary.AddForce(rigidbody, new AddForceVars(direction, vars.ForceMode));
        }
    }
    [System.Serializable]
    public class FallingVariablesRB
    {
        public LayerMask GroundLayer;
        public float FallingForce;
        public AnimationCurve FallingCurve;
        public float TimeToMaxFallSpeed;
        public Vector3 GravityDirection;
        public ForceMode ForceMode;
        public FallingVariablesRB(Vector3 gravitydir, LayerMask ground, float fallingForce, float timeToMaxForce, AnimationCurve curve, ForceMode mode = ForceMode.Force)
        {
            GroundLayer = ground;
            GravityDirection = gravitydir;
            FallingForce = fallingForce;
            TimeToMaxFallSpeed = timeToMaxForce;
            FallingCurve = curve;
            ForceMode = mode;
        }

    }
}