using GWLPXL.Movement.com;
using UnityEngine;

namespace GWLPXL.Movement.Character.CC.com
{
    /// <summary>
    /// base class for the falling variables on the character CC
    /// </summary>
    [System.Serializable]
    public class FallingVariablesCC
    {
        public float Multiplier = 1;
        public LayerMask GroundLayer;
        public float FallingSpeed;
        public AnimationCurve FallingCurve;
        public float TimeToMaxFallSpeed;
        public Vector3 GravityDirection;
        public CCOptions Type;
        public bool IsGrounded;
        public float CurrentFallingSpeed;

        public FallingVariablesCC(Vector3 gravitydir, LayerMask ground, float fallingspeed, float timetomaxspeed, AnimationCurve curve, CCOptions type = CCOptions.SimpleMove, float multi = 1)
        {
            GroundLayer = ground;
            GravityDirection = gravitydir;
            FallingSpeed = fallingspeed;
            TimeToMaxFallSpeed = timetomaxspeed;
            FallingCurve = curve;
            Type = type;
            Multiplier = multi;
        }

    }
    /// <summary>
    /// falling class for the character controller. 
    /// </summary>
    public class FallingCC : ITick
    {
        public System.Action OnLanded;
        public System.Action OnAirborne;
        FallingVariablesCC vars;
        CharacterController controller;
        CapsuleCollider capsule;
        float timer;
        bool cachegrounded;
        public FallingCC(CharacterController controller, FallingVariablesCC vars)
        {
            this.vars = vars;
            this.controller = controller;
            this.capsule = controller.GetComponent<CapsuleCollider>();
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
            FallingBehavior();

            GroundCheck();

        }

        protected virtual void FallingBehavior()
        {
            if (controller.enabled == false || controller.isGrounded || vars.Multiplier == 0)
            {
                timer = 0;

            }
            float dt = GetTickDuration();
            timer += dt;

            float percent = timer / vars.TimeToMaxFallSpeed;
            if (vars.FallingCurve != null)
            {
                percent = vars.FallingCurve.Evaluate(percent);
            }
            float newY = percent * vars.FallingSpeed;// * vars.Multiplier;// * GetTickDuration();

            vars.CurrentFallingSpeed = newY * vars.Multiplier;// * GetTickDuration();

            switch (vars.Type)
            {
                case CCOptions.SimpleMove:
                    MovementPrimary.SimpleMoveCC(controller, vars.GravityDirection * vars.CurrentFallingSpeed * dt);
                    break;
                case CCOptions.Move:
                    MovementPrimary.MoveCC(controller, vars.GravityDirection * vars.CurrentFallingSpeed * dt);
                    break;
            }
        }

        protected virtual void GroundCheck()
        {
            vars.IsGrounded = Detection.SimpleSpherecast(capsule.bounds.max, capsule.radius * .9f, vars.GravityDirection, capsule.height * .9f, vars.GroundLayer);
            if (vars.IsGrounded != cachegrounded)
            {
                cachegrounded = vars.IsGrounded;
                if (cachegrounded)
                {
                    OnLanded?.Invoke();
                }
                else
                {
                    OnAirborne?.Invoke();
                }
            }
        }

       
    }

}