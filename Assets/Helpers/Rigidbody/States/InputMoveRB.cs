using GWLPXL.Movement.com;
using UnityEngine;

namespace GWLPXL.Movement.RB.com
{
    public class InputMoveRB : ITick
    {
        Rigidbody rigidbody;
        InputMoveVarsRB vars;
        public InputMoveRB(Rigidbody rigidbody, InputMoveVarsRB vars)
        {
            this.rigidbody = rigidbody;
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
            Vector3 newMove = new Vector3(vars.X, rigidbody.velocity.y, vars.Z).normalized;

            Vector3 translatedInput = newMove;
            switch (vars.Reference)
            {
                case InputReference.Camera:
                    translatedInput = MovementPrimary.TranslateToCamera(newMove);
                    break;
                case InputReference.Local:
                    translatedInput = MovementPrimary.TranslateToLocal(rigidbody.transform, newMove);
                    break;
            }

            switch (vars.Type)
            {
                case RigibodyMoveType.AddForce:
                    MovementPrimary.AddForce(rigidbody, new AddForceVars(translatedInput * vars.Force * GetTickDuration(), vars.ForceMode));
                    break;
                case RigibodyMoveType.AddVelocity:
                    MovementPrimary.AddVelocity(rigidbody, new VelocityVars(translatedInput, vars.Force));
                    break;
                case RigibodyMoveType.SetVelocity:
                    MovementPrimary.SetVelocity(rigidbody, translatedInput * vars.Force);
                    break;
                case RigibodyMoveType.MovePosition:
                    MovementPrimary.MovePosition(rigidbody, new MovePositionVars(rigidbody.position + translatedInput * vars.Force * GetTickDuration()));
                    break;
            }

            RemoveTicker();
        }


    }
    [System.Serializable]
    public class InputMoveVarsRB
    {
        public float X;
        public float Z;
        public float Force = 55;
        public RigibodyMoveType Type = RigibodyMoveType.AddForce;
        public ForceMode ForceMode = ForceMode.Force;
        public InputReference Reference = InputReference.Camera;
        public InputMoveVarsRB(float x, float y, float z, float speed, RigibodyMoveType type = RigibodyMoveType.AddForce, ForceMode force = ForceMode.Force, InputReference reference = InputReference.Camera)
        {
            X = x;
            Z = z;
            Force = speed;
            Type = type;
            ForceMode = force;
            Reference = reference;
        }
    }

}
