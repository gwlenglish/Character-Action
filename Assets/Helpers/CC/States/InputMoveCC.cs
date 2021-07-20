
using GWLPXL.Movement.Character.com;
using GWLPXL.Movement.com;
using UnityEngine;

namespace GWLPXL.Movement.Character.CC.com
{
    public enum CCOptions
    {
        SimpleMove = 0,
        Move = 1
    }
    /// <summary>
    /// base class for the input vars for a CC
    /// </summary>
    [System.Serializable]
    public class InputMoveVarsCC
    {
        public float Multiplier = 1;
        public float AirborneMulti = 1;
        public float X;
        public float Z;
        public float Speed;
        public CCOptions Type;
        public InputReference Reference;
        public Vector3 MoveDirection;//readonly
        public Vector3 LocalInput;
        public Vector3 GlobalInput;
        public InputMoveVarsCC(float x, float z, float speed, CCOptions type = CCOptions.SimpleMove, InputReference reference = InputReference.Camera, float muli = 1)
        {
            X = x;
            Z = z;
            Speed = speed;
            Type = type;
            Reference = reference;
            Multiplier = muli;
        }
    }
    /// <summary>
    /// ticker class to gather inputs for the CC
    /// </summary>
    public class InputMoveCC : ITick
    {
        CharacterController controller;
        PlayerControllerCC controls;
        float timer;
        public InputMoveCC(CharacterController controller, PlayerControllerCC controls)
        {
            this.controls = controls;
            this.controller = controller;
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
            float dt = GetTickDuration();
            timer += dt;

            MovingBehavior(dt);

        }

        private void MovingBehavior(float dt)
        {
            FreeFormState state = ActionManager.GetCharacterStateCC(controls);
            InputMoveVarsCC vars = controls.Controls.Controller.Locomotion.Movement;
            float multi = vars.Multiplier;
            switch (state)
            {
                case FreeFormState.Airborne:
                    multi = vars.AirborneMulti;
                    break;
            }

            Vector3 newMove = new Vector3(vars.X, 0, vars.Z).normalized * vars.Speed * multi * dt;
            vars.GlobalInput.x = vars.X;
            vars.GlobalInput.z = vars.Z;
            vars.LocalInput.x = vars.X;
            vars.LocalInput.z = vars.Z;
            vars.LocalInput = MovementPrimary.TranslateToLocal(controller.transform, vars.LocalInput);
            Vector3 translated = newMove;
            switch (vars.Reference)
            {
                case InputReference.Camera:
                    translated = MovementPrimary.TranslateToCamera(newMove);
                    break;
                case InputReference.Local:
                    translated = MovementPrimary.TranslateToLocal(controller.transform, newMove);
                    break;
            }


            switch (vars.Type)
            {
                case CCOptions.SimpleMove:
                    MovementPrimary.SimpleMoveCC(controller, translated);
                    break;
                case CCOptions.Move:
                    MovementPrimary.MoveCC(controller, translated);
                    break;
            }

            vars.MoveDirection = translated;
        }
    }

  

   

}
