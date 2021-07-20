
using GWLPXL.Movement.com;
using UnityEngine;
using System.Collections.Generic;
using GWLPXL.Movement.Character.com;


namespace GWLPXL.Movement.Character.CC.com
{
   /// <summary>
   /// The main action state for the character controller. Controls the state's current info and checks next transition conditions
   /// </summary>
    public class ActionCC : ITick
    {
        public System.Action<CharacterController, PlayerControllerCC, Animator, ActionCC> OnComplete;
        ActionCCVars vars;
        Animator animator;
        CharacterController controller;
        PlayerControllerCC controls;
        Vector3 start;
        Vector3 goal;
        float timer;

        float goalx;
        float goaly;
        float goalz;

        int nextmove = -1;

        public ActionCC(CharacterController controller, PlayerControllerCC playerC, Animator animator, ActionCCVars vars)
        {
            this.vars = vars;
            this.controller = controller;
            this.controls = playerC;
            this.animator = animator;
            start = Vector3.zero;
            goalx =  start.x + vars.TravelX.Distance;
            goaly =  start.y +  vars.TravelY.Distance;
            goalz =   start.z +  vars.TravelZ.Distance;

            goal = new Vector3(goalx, goaly, goalz);

            AddTicker();
        }
        public void AddTicker()
        {
            timer = 0;
            animator.GetComponent<IRootMotion>().SetRootMotionActive(vars.AnimatorVars.ApplyRootMotion);
            animator.speed = vars.AnimatorVars.AnimatorSpeed;
            animator.CrossFade(vars.AnimatorVars.AnimatorStateName, vars.AnimatorVars.CrossFadeDuration, 0, 0);
            controls.Controls.Controller.Locomotion.Movement.Multiplier = vars.Multipliers.MoveMulti;
            controls.Controls.Controller.Rotate.Rotation.Multiplier = vars.Multipliers.RotateMulti;
            controls.Controls.Controller.Fall.Falling.Multiplier = vars.Multipliers.FallMulti;

            TickManager.AddTicker(this);
        }

        public float GetTickDuration()
        {
            return Time.deltaTime;
        }

        public void RemoveTicker()
        {
            ResetActionObject();
            TickManager.RemoveTicker(this);

        }

        protected virtual void ResetActionObject()
        {
            animator.GetComponent<IRootMotion>().SetRootMotionActive(CharacterDefaults.UseRoot);
            controls.Controls.Controller.Locomotion.Movement.Multiplier = CharacterDefaults.MoveMulti;
            controls.Controls.Controller.Rotate.Rotation.Multiplier = CharacterDefaults.RotateMulti;
            controls.Controls.Controller.Fall.Falling.Multiplier = CharacterDefaults.FallMulti;
            animator.speed = CharacterDefaults.AnimatorSpeed;
            timer = 0;
            OnComplete?.Invoke(controller, controls, animator, this);
        }

        protected virtual void Interrupt(int newMoveIndex)
        {
            if (newMoveIndex > -1)
            {
                ActionManager.ContinueFlowSequence(controller, controls, animator, this, newMoveIndex);
            }
     
        }
        //need to gate.
        public void Tick()
        {

            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName(vars.AnimatorVars.AnimatorStateName) == false) return;

            Debug.Log("State Length " + state.length);
            float dt = GetTickDuration();
            float totald = state.length + vars.AdditionalDuration;// + vars.AnimatorVars.CrossFadeDuration;//issues with blend trees?
            timer += dt;//if extending

            float percent = timer / totald;
            Debug.Log("PERCENT " + percent);
            //check blocking
            CheckBlocking();
            //next transition, input and action early exit
            CheckEarlyExit(percent);

            CheckExtending(dt);

            //next transition input gathering
            CollectBufferInputs(totald);


            //next transition, action
            TryNextAction(totald);


            //movement behavior

            float xpercent = percent;
            float ypercent = percent;
            float zpercent = percent;
            CalculateCurves(timer, ref xpercent, ref ypercent, ref zpercent);

            float lerpx = Mathf.Lerp(start.x, goal.x, xpercent);
            float lerpy = Mathf.Lerp(start.y, goal.y, ypercent);
            float lerpz = Mathf.Lerp(start.z, goal.z, zpercent);

            Vector3 x = Vector3.Lerp(start, new Vector3(lerpx, start.y, start.z), xpercent);
            Vector3 y = Vector3.Lerp(start, new Vector3(start.x, lerpy, start.z), ypercent);
            Vector3 z = Vector3.Lerp(start, new Vector3(start.x, start.y, lerpz), zpercent);
            Vector3 newmove = x + y + z;


            newmove = controller.transform.TransformDirection(newmove);

            switch (vars.Type)
            {
                case CCOptions.Move:
                    MovementPrimary.MoveCC(controller, newmove * dt);
                    break;
                case CCOptions.SimpleMove:
                    MovementPrimary.SimpleMoveCC(controller, newmove);
                    break;
            }



            if (timer / totald >= 1)
            {

                RemoveTicker();
                return;
            }



        }

        protected virtual void TryNextAction(float totald)
        {
            if (timer >= vars.AnimatorVars.MinTransition * totald && nextmove > -1)
            {
                CharacterActionsCCSO extras = controls.Controls.Actions[nextmove];
                if (controls.Controls.FlowControls.CanTransition(extras.Movement.Movements.ScriptedMovement.Name))
                {
                    ActionManager.ContinueFlowSequence(controller, controls, animator, this, nextmove);//perform input
                }

            }
        }

        protected virtual void CollectBufferInputs(float totald)
        {
            if (timer >= totald * vars.BufferVars.MinBufferInputNormalizedTime && timer <= totald * vars.BufferVars.MaxBufferInputNormalziedTime)
            {
                ActionManager.GetActionInputRequirements(controls, Interrupt);//collect input
            }
        }

        protected virtual void CheckExtending(float dt)
        {
            if (vars.ExtendOptions.UseExtend)
            {
                bool extend = ActionManager.InputSuccess(vars.ExtendOptions.ExtendActionInput, controls.Controls.Controller.Locomotion.Movement);
                if (extend)
                {
                    timer -= dt;
                }

                if (vars.ExtendOptions.ShouldExitOnFail && extend == false)
                {
                    RemoveTicker();
                }
            }
        }

        protected virtual void CheckBlocking()
        {
            if (vars.AnimatorVars.ApplyRootMotion && vars.BlockingOptions.Blocking.Count > 0)
            {

                //we gotta collision check ourselves...
                Vector3 floor = controller.transform.position;
                Vector3 head = controller.transform.position + new Vector3(0, controller.height, 0);
                for (int i = 0; i < vars.BlockingOptions.Blocking.Count; i++)
                {
                    Blocking blocking = vars.BlockingOptions.Blocking[i];
                    bool overlap = Detection.SimpleCapsuleCast(floor, head, controller.radius, controller.transform.TransformDirection(blocking.Direction), blocking.DistanceCheck, blocking.BlockingLayers);
                    if (overlap)
                    {
                        //exit
                        ActionManager.FlowSequenceComplete(controller, controls, animator, this);
                        break;
                    }
                }


            }
        }

        protected virtual void CheckEarlyExit(float percent)
        {

            int exit = ActionManager.GetActionInputRequirements(controls, true, false);
            if (exit > -1)
            {
                CharacterActionsCCSO extras = controls.Controls.Actions[exit];
                string name = extras.Movement.Movements.ScriptedMovement.Name;
                for (int i = 0; i < vars.EarlyExitOptions.EarlyExits.Count; i++)
                {
                    string earlyexit = vars.EarlyExitOptions.EarlyExits[i].AnimatorStateName;
                    
                    if (string.CompareOrdinal(name, earlyexit) == 0)
                    {
                        //same as current.
                        //early exit
                        if (percent >= vars.EarlyExitOptions.EarlyExits[i].CanExitTime)
                        {
                            Interrupt(exit);
                            break;
                        }
     
                    }
                }

            }

        }
       

       
       
      
        

        protected virtual void CalculateCurves(float timer, ref float xpercent, ref float ypercent, ref float zpercent)
        {
            if (vars.TravelX.Curve != null)
            {
                xpercent = vars.TravelX.Curve.Evaluate(xpercent);
            }else if (vars.TravelX.Duration > 0)
            {
                xpercent = timer / vars.TravelX.Duration;
            }

            if (vars.TravelY.Curve != null)
            {
                ypercent = vars.TravelY.Curve.Evaluate(ypercent) ;
            }
            else if (vars.TravelY.Duration > 0)
            {
                ypercent = timer / vars.TravelY.Duration;
            }

            if (vars.TravelZ.Curve != null)
            {
                zpercent = vars.TravelZ.Curve.Evaluate(zpercent) ;
            }
            else if (vars.TravelZ.Duration > 0)
            {
                zpercent = timer / vars.TravelZ.Duration;
            }
        }
    }

    #region helpers
    /// <summary>
    /// distance variables for the scripted movement
    /// </summary>
    [System.Serializable]
    public class Distances
    {
        [Tooltip("Local distance, negatives go in reverse.")]
        public float Distance;
        public float Duration;
        public AnimationCurve Curve;

        public Distances(float d, float duration, AnimationCurve curve = null)
        {
            Duration = duration;
            Distance = d;
            Curve = curve;
        }
    }
    /// <summary>
    /// early exit variables
    /// </summary>
    [System.Serializable]
    public class EarlyExitAction
    {
        public bool ApplyRootMotion = false;
        public string AnimatorStateName = string.Empty;
        public float CrossFadeDuration = .25f;
        [Tooltip("Normalized time between 0 and 1")]
        [Range(0, 1)]
        public float CanExitTime = 1;
    }
    /// <summary>
    /// early exit state data
    /// </summary>
    [System.Serializable]
    public class EarlyExitState
    {
        public FreeFormState State = default;
        public float CrossFadeDuration = .25f;
        [Range(0, 1)]
        public float CanExitTime = 1;
        public bool ApplyRootMotion = false;

    }
    /// <summary>
    /// base class for early exit options
    /// </summary>
    [System.Serializable]
    public class EarlyExitOptions
    {
        public EarlyExitState[] EarlyExitStates = new EarlyExitState[0];
        public List<EarlyExitAction> EarlyExits = new List<EarlyExitAction>();
    }
    /// <summary>
    /// animator vars for the action
    /// </summary>
    [System.Serializable]
    public class AnimatorVars
    {
        public bool ApplyRootMotion = false;
        public string AnimatorStateName = string.Empty;
        public float CrossFadeDuration = .25f;
        [Tooltip("Percentage")]
        [Range(0f, 1f)]
        public float MinTransition = .25f;
        public float AnimatorSpeed = 1;

    }
    /// <summary>
    /// input buffer variables
    /// </summary>
    [System.Serializable]
    public class InputBufferVars
    {
        [Header("Transition Buffers")]
        [Range(0, 1f)]
        public float MinBufferInputNormalizedTime = .25f;
        [Range(0, 1f)]
        public float MaxBufferInputNormalziedTime = .75f;

        public InputBufferVars(float min, float max)
        {
            MinBufferInputNormalizedTime = min;
            MaxBufferInputNormalziedTime = max;
        }
    }
    /// <summary>
    /// blocking variables
    /// </summary>
    [System.Serializable]
    public class Blocking
    {
        public LayerMask BlockingLayers;
        public float DistanceCheck = 1;
        public Vector3 Direction = new Vector3(0, 0, 1);

    }
    /// <summary>
    /// list of blocking options
    /// </summary>
    [System.Serializable]
    public class BlockingOptions
    {
        public List<Blocking> Blocking = new List<Blocking>();
    }
    /// <summary>
    /// extend options for actions
    /// </summary>
    [System.Serializable]
    public class ExtendOptions
    {
        public bool UseExtend = false;
        public InputRequirements ExtendActionInput = new InputRequirements();
        public bool ShouldExitOnFail = true;
    }
    /// <summary>
    /// action variables for the action state
    /// </summary>
    [System.Serializable]
    public class ActionCCVars
    {
        public BlockingOptions BlockingOptions = new BlockingOptions();
        public MovementMultipliers Multipliers = new MovementMultipliers();
        public Distances TravelX;
        public Distances TravelY;
        public Distances TravelZ;
        public AnimatorVars AnimatorVars = new AnimatorVars();
        [Tooltip("Additional duration over the Animation length")]
        public float AdditionalDuration = 0;
        public InputBufferVars BufferVars = new InputBufferVars(.25f, .75f);
        public CCOptions Type = CCOptions.Move;
        public ExtendOptions ExtendOptions = new ExtendOptions();
        public EarlyExitOptions EarlyExitOptions = new EarlyExitOptions();
        public ActionCCVars(float duration, CCOptions type = CCOptions.Move)
        {
            AdditionalDuration = duration;
            Type = type;
        }
    }

    #endregion
}