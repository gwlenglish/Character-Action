using System.Collections.Generic;
using UnityEngine;
using System;
using GWLPXL.Movement.Character.com;
using GWLPXL.Movement.com;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.Character.CC.com
{
    /// <summary>
    /// used to manager the character actions and flow
    /// </summary>
    public static class ActionManager
    {
        public static Action<CharacterController> OnActionComplete;
        public static Action<CharacterController> OnActionStart;


        static Dictionary<PlayerControllerCC, CharacterController> pair = new Dictionary<PlayerControllerCC, CharacterController>();
        static Dictionary<int, ActionTracker> scriptedDic = new Dictionary<int, ActionTracker>();

        public static void GetActionInputRequirements(PlayerControllerCC player, System.Action<int> onSuccess, bool checkinput = true, bool checktransition = true)
        {
            int index = GetActionInputRequirements(player, checkinput, checktransition);
            Debug.Log("Action index" + index);
            if (index > -1)
            {
                Debug.Log("start action");
                onSuccess?.Invoke(index);
            }
        }
        public static int GetActionInputRequirements(PlayerControllerCC player, bool checkinput = true, bool checktransition = true)
        {
            CharacterActionsCCSO[] extras = player.Controls.Actions;
            Dictionary<int, ExtraMovementCC> poss = new Dictionary<int, ExtraMovementCC>();
            for (int i = 0; i < extras.Length; i++)
            {
                ExtraMovementCC movementCC = extras[i].Movement.Movements;
                if (CharacterActionRequirementsSuccess(player, movementCC) == false)
                {
                    continue;
                }

                if (checkinput == true)
                {
                    InputRequirements requirements = extras[i].Movement.Movements.ScriptedMovement.InputRequirements;
                    if (InputSuccess(requirements, player.Controls.Controller.Locomotion.Movement))
                    {

                        //if can transition to this state, reutrn true
                        poss[i] = movementCC;
                    }
                }
            }


            ExtraMovementCC priority = null;
            int highest = -1;
            int index = -1;
            foreach (var kvp in poss)
            {
                Debug.Log("Possible " + kvp.Value.ScriptedMovement.Name);
                if (kvp.Value.ScriptedMovement.Priority > highest )
                {
                    if (checktransition)
                    {
                        if (player.Controls.FlowControls.CanTransition(kvp.Value.ScriptedMovement.Name) == true)
                        {
                            index = kvp.Key;
                            highest = kvp.Value.ScriptedMovement.Priority;
                            priority = kvp.Value;
                        }
                    }
                    else
                    {
                        index = kvp.Key;
                        highest = kvp.Value.ScriptedMovement.Priority;
                        priority = kvp.Value;
                    }
        
                }

            }
            Debug.Log(index);
            Debug.Log(highest);
            if (priority != null)
            {
                Debug.Log("Priority " + priority.ScriptedMovement.Name);
                Debug.Log("Index " + index);
            }
            return index;
        }
       
        public static bool InputSuccess(InputRequirements requirements, InputMoveVarsCC move)
        {
            if (requirements.InputButtons.Length == 0 && requirements.AxisRequirements.Length == 0) return false;

            InputButton[] buttons = requirements.InputButtons;
            int buttonamount = 0;
            buttonamount = UnityLegacyButtons(buttons, buttonamount);
            if (buttonamount != buttons.Length)
            {
                //fail
                return false;
            }


            InputAxisFreeForm[] axes = requirements.AxisRequirements;
            int axisamount = 0;
            for (int j = 0; j < axes.Length; j++)
            {
                float minthresh = axes[j].MinDirectionThreshold;
                AxisRequirementType type = axes[j].RequirementType;
                float evaluatedaxis = -100f;//keep, if stays at -100f then failed to evaluate
                //switch case needs re-evaluation, probably dont need local inputs unless there's a way to stop movement like strafe
                switch (type)
                {
                    case AxisRequirementType.LocalInputZPositive:
                        evaluatedaxis = move.LocalInput.z;

                        break;
                    case AxisRequirementType.LocalInputZZero:
                        evaluatedaxis = move.LocalInput.z;

                        break;
                    case AxisRequirementType.LocalInputZNegative:
                        evaluatedaxis = move.LocalInput.z;

                        break;
                    case AxisRequirementType.LocalInputXPositive:
                        evaluatedaxis = move.LocalInput.x;

                        break;
                    case AxisRequirementType.LocalInputXNegative:
                        evaluatedaxis = move.LocalInput.x;

                        break;
                    case AxisRequirementType.LocalInputXZero:
                        evaluatedaxis = move.LocalInput.x;

                        break;
                    case AxisRequirementType.GlobalInputXNegative:
                        evaluatedaxis = move.GlobalInput.x;

                        break;
                    case AxisRequirementType.GlobalInputXPositive:
                        evaluatedaxis = move.GlobalInput.x;

                        break;
                    case AxisRequirementType.GlobalInputXZero:
                        evaluatedaxis = move.GlobalInput.x;

                        break;
                    case AxisRequirementType.GlobalInputZNegative:
                        evaluatedaxis = move.GlobalInput.z;

                        break;
                    case AxisRequirementType.GlobalInputZPositive:
                        evaluatedaxis = move.GlobalInput.z;

                        break;
                    case AxisRequirementType.GlobalInputZZero:
                        evaluatedaxis = move.GlobalInput.z;

                        break;

                }

                if (Mathf.Abs(evaluatedaxis) >= minthresh && evaluatedaxis != -100f)
                {
                    axisamount++;
                }
            }

            if (axisamount != axes.Length)
            {
                //axis combo error
                //fail
                return false;
            }

            if (axisamount == 0 && buttonamount == 0)
            {
                return false;
            }
            return true;

        }

        private static int UnityLegacyButtons(InputButton[] buttons, int buttonamount)
        {
            for (int j = 0; j < buttons.Length; j++)
            {
                string name = buttons[j].ButtonName;
                ButtonType type = buttons[j].Type;
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                switch (type)
                {
                    case ButtonType.Click:
                        if (Input.GetButtonDown(name))
                        {
                            buttonamount++;
                        }
                        break;
                    case ButtonType.Held:
                        if (Input.GetButton(name))
                        {
                            buttonamount++;
                        }
                        break;
                    case ButtonType.Released:
                        if (Input.GetButtonUp(name))
                        {
                            buttonamount++;
                        }
                        break;
                }

            }

            return buttonamount;
        }

        private static bool CharacterActionRequirementsSuccess(PlayerControllerCC player, ExtraMovementCC movementCC)
        {
            FreeFormState current = GetCharacterStateCC(player);
            for (int j = 0; j < movementCC.ScriptedMovement.CharacterRequirements.RequiredStates.Length; j++)
            {
                FreeFormState required = movementCC.ScriptedMovement.CharacterRequirements.RequiredStates[j];
                if (current == required)
                {
                    return true;
                }
            }
            return false;
        }


        public static FreeFormState GetCharacterStateCC(PlayerControllerCC player)
        {
            FreeFormState state = FreeFormState.Ground;
            if (player.Controls.Controller.Fall.Falling.IsGrounded == false)
            {
                state = FreeFormState.Airborne;

                //see if we are hitting the ground soon.
                //would need instance
            }

            

            return state;
        }



        public static bool InActionSequence(PlayerControllerCC character)
        {
            return pair.ContainsKey(character);

        }

        public static void ContinueFlowSequence(CharacterController charcontroller, PlayerControllerCC movementCC, Animator animator, ActionCC interrupted, int newsequence)
        {
            if (scriptedDic.ContainsKey(charcontroller.GetInstanceID()))
            {
                //pair.Remove(movementCC);
                ActionTracker tracker = scriptedDic[charcontroller.GetInstanceID()];
                tracker.CurrentMove.OnComplete -= ContinueFlowSequence;
                scriptedDic.Remove(charcontroller.GetInstanceID());
                tracker.CurrentMove.RemoveTicker();
                Debug.Log("Interrupted");
                StartFlowSequence(charcontroller, movementCC, newsequence, animator);
                
    
            }
            else
            {
                Debug.Log("Not found");
            }
        }
        public static void StartFlowSequence(CharacterController charcontroller, PlayerControllerCC movementCC, int extraMoveIndex, Animator animator = null)
        {
          
            ActionCCVars[] entiresequence = movementCC.Controls.Actions[extraMoveIndex].Movement.Movements.ScriptedMovement.MovementBehavior.MovementSequence;
            string current = movementCC.Controls.Actions[extraMoveIndex].Movement.Movements.ScriptedMovement.Name;
            if (movementCC.Controls.FlowControls != null)
            {
                movementCC.Controls.FlowControls.Current = current;
            }

            ActionCC dashCC = MovementPrimary.ScriptedMoveCC(charcontroller, movementCC, animator, entiresequence[0]);
            dashCC.OnComplete += ContinueFlowSequence;
            int index = 0;
            scriptedDic[charcontroller.GetInstanceID()] = new ActionTracker(current, index, entiresequence, animator, dashCC);
            pair[movementCC] = charcontroller;
            OnActionStart?.Invoke(charcontroller);

            Debug.Log("Started " + movementCC.Controls.Actions[extraMoveIndex].Movement.Movements.ScriptedMovement.Name);
        }

        //need to use controls meaningfully
        static void ContinueFlowSequence(CharacterController controller, PlayerControllerCC controls, Animator animator, ActionCC old)
        {
            if (scriptedDic.ContainsKey(controller.GetInstanceID()) == false) return;//not found

            old.OnComplete -= ContinueFlowSequence;
            ActionTracker tracker = scriptedDic[controller.GetInstanceID()];
            tracker.Index++;
            if (tracker.Index > tracker.CurrentSequence.Length - 1)
            {
                //complete
                FlowSequenceComplete(controller, controls, animator, old);
            }
            else
            {
           
                ActionCC dashCC = MovementPrimary.ScriptedMoveCC(controller, controls, animator, tracker.CurrentSequence[tracker.Index]);
                tracker.CurrentMove = dashCC;
                dashCC.OnComplete += ContinueFlowSequence;
                Debug.Log("Continued " + tracker.Name);

            }
        }

        public static void FlowSequenceComplete(CharacterController controller, PlayerControllerCC controls, Animator animator, ActionCC old)
        {
   
            if (scriptedDic.ContainsKey(controller.GetInstanceID()))
            {
                pair.Remove(controls);
                old.OnComplete -= ContinueFlowSequence;
                old.RemoveTicker();
                ActionTracker tracker = scriptedDic[controller.GetInstanceID()];
                scriptedDic.Remove(controller.GetInstanceID());
                //reset current
                controls.Controls.FlowControls.Current = string.Empty;
                OnActionComplete?.Invoke(controller);
                Debug.Log("Ended " + tracker.Name);


                //interrupt state
                FreeFormState state = GetCharacterStateCC(controls);
                switch (state)
                {
                    case FreeFormState.Airborne:
                        animator.CrossFade(CharacterDefaults.Airborne, CharacterDefaults.AirborneTrans, 0, 0);
                        break;
                    case FreeFormState.Ground:
                        animator.CrossFade(CharacterDefaults.Locomotion, CharacterDefaults.LocomotionTrans, 0, 0);
                        break;
                }
            }
                

        }

         [System.Serializable]
        public class ActionTracker
        {
            public string Name;
            public Animator Animator;
            public int Index;
            public ActionCCVars[] CurrentSequence;
            public ActionCC CurrentMove;
            public ActionTracker(string name, int index, ActionCCVars[] seq, Animator animator, ActionCC currentMove)
            {
                Name = name;
                Index = index;
                CurrentSequence = seq;
                Animator = animator;
                CurrentMove = currentMove;
            }
        }


    }



}