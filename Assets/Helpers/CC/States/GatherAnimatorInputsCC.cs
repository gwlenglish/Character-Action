using GWLPXL.Movement.Character.com;
using GWLPXL.Movement.com;
using UnityEngine;

namespace GWLPXL.Movement.Character.CC.com
{


    /// <summary>
    /// ticker class to gather animator inputs for the CC
    /// </summary>
    public class GatherAnimatorInputsCC : ITick
    {
        PlayerControllerCC controls;
        Animator animator;


        public GatherAnimatorInputsCC(PlayerControllerCC player, Animator animator)
        {
            this.controls = player;
            this.animator = animator;

            AddTicker();
        }
        public void AddTicker()
        {
            Ini();
            TickManager.AddTicker(this);
        }

        protected virtual void TransitionBack()
        {
            controls.Controls.Controller.Locomotion.Movement.Multiplier = CharacterDefaults.MoveMulti;
            controls.Controls.Controller.Rotate.Rotation.Multiplier = CharacterDefaults.RotateMulti;
            animator.GetComponent<IRootMotion>().SetRootMotionActive(CharacterDefaults.UseRoot);
            FreeFormState state = ActionManager.GetCharacterStateCC(controls);
            switch (state)
            {
                case FreeFormState.Ground:
                    animator.CrossFade(controls.Controls.Controller.Locomotion.Animator.AnimatorStateName, CharacterDefaults.LocomotionTrans, 0, 0);
                    break;
                case FreeFormState.Airborne:
                    animator.CrossFade(controls.Controls.Controller.Locomotion.Animator.AirborneState, CharacterDefaults.AirborneTrans, 0, 0);
                    break;
            }
        }
        protected virtual void Ini()
        {
            TransitionBack();


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
            AssignAnimatorParams();
        }

        protected virtual void AssignAnimatorParams()
        {
            MovementPrimary.AssignAnimatorParamInputsCC(controls, animator);
        }

       
    }

}