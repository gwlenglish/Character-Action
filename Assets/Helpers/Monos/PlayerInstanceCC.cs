using GWLPXL.Movement.Character.com;
using GWLPXL.Movement.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.Movement.Character.CC.com
{
    public interface ICharacterInstanceCC
    {
        CharacterControllerSO Controls { get; set; }
        Animator Animator { get; set; }
        CharacterController CharController { get; set; }
    }
    [System.Serializable]
    public class PlayerCacheStatesCC
    {
        public FallingCC Falling;
        public InputMoveCC Move;
        public InputRotate Rotate;
        public GatherPlayerStateInputs Inputs;
        public GatherAnimatorInputsCC AnimParams;
        public GatherSequenceInputs SequenceInputs;
    }
    public class PlayerInstanceCC : MonoBehaviour, ICharacterInstanceCC
    {
        public CharacterControllerSO Controls { get => Character; set => Character = value; }
        Animator ICharacterInstanceCC.Animator { get => Animator; set => Animator = value; }
        public CharacterController CharController { get => Controller; set => Controller = value; }

        public CharacterControllerSO Character;
        public CharacterController Controller;
        public Animator Animator;

        PlayerCacheStatesCC states;
        bool inputdisabled;

        #region lifteime
        protected virtual void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            if (Animator == null)
            {
                Debug.LogError("Needs an animator attached as a child");
            }
        }
        protected virtual void Start()
        {
            if (states == null)
            {
                states = MovementPrimary.PlayerStateConstructionCC(Controller, Character, Animator, StartSequence);
                Controls.PlayerCC.Controls.FlowControls.Current = string.Empty;
            }
        }

        protected virtual void OnDestroy()
        {
            MovementPrimary.RemoveStatesCC(states);
        }
        #endregion

        #region public
        public virtual void MakeScriptedSequenceClone(CharacterControllerSO runtimecopy)
        {
            
            DisableInput(true);
            Character = runtimecopy;
            MovementPrimary.RemoveStatesCC(states);
            states = MovementPrimary.PlayerStateConstructionCC(Controller, Character, Animator, StartSequence);
            PlayerInstancePreviewCC copypreview = GetComponent<PlayerInstancePreviewCC>();
            if (copypreview != null) Destroy(copypreview);

        }
        public virtual void DisableInput(bool disabled)
        {
            if (states == null)
            {
                states = MovementPrimary.PlayerStateConstructionCC(Controller, Character, Animator, StartSequence);
            }
            inputdisabled = disabled;
            if (inputdisabled)
            {
                states.AnimParams.RemoveTicker();
                states.SequenceInputs.RemoveTicker();
            }
            else
            {
                states.AnimParams.AddTicker();
                states.SequenceInputs.AddTicker();
            }
        }

        #endregion

        protected virtual void StartSequence(int index)
        {
            DisableInput(true);
            Animator.GetComponent<IRootMotion>().SetRootMotionActive(CharacterDefaults.UseRoot);
            ActionManager.StartFlowSequence(Controller, Character.PlayerCC, index, Animator);
            ActionManager.OnActionComplete += SequenceComplete;

            Debug.Log("Sequence started");


        }
        protected virtual void SequenceComplete(CharacterController cont)
        {
            if (cont != Controller) return;
            ActionManager.OnActionComplete -= SequenceComplete;

            DisableInput(false);

        }
        


    }

}