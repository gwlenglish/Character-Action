using GWLPXL.Movement.com;
using UnityEngine;

namespace GWLPXL.Movement.RB.com
{
    public class InputRotateRB : ITick
    {
        Rigidbody rigidbody;
        InputRotateVarsRB vars;
        public InputRotateRB(Rigidbody rigidbody, InputRotateVarsRB vars)
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
            float dt = GetTickDuration();
            Vector3 newMove = new Vector3(vars.X, 0, vars.Z);
            Vector3 translatedInput = newMove;
            switch (vars.Reference)
            {
                case InputReference.Camera:
                    translatedInput = MovementPrimary.TranslateToCamera(newMove);
                    break;
            }

            Quaternion applied = MovementPrimary.GetLookRotation(rigidbody, translatedInput, vars.Speed,dt, vars.Smooth);
            switch (vars.Type)
            {
                case RigibodyRotateType.MoveRotation:
                    MovementPrimary.MoveRotation(rigidbody, new RotateRBVars(applied));
                    break;
                case RigibodyRotateType.SetRotation:
                    MovementPrimary.SetRotation(rigidbody, new RotateRBVars(applied));
                    break;
            }
            RemoveTicker();
        }

        
    }

    [System.Serializable]
    public class InputRotateVarsRB
    {
        public float X;
        public float Z;
        public float Speed = 15;
        public RigibodyRotateType Type = RigibodyRotateType.MoveRotation;
        public RotateSmoothType Smooth = RotateSmoothType.Slerp;
        public InputReference Reference = InputReference.Camera;

        public InputRotateVarsRB(float x, float y, float z, float speed, RigibodyRotateType type = RigibodyRotateType.MoveRotation, RotateSmoothType smooth = RotateSmoothType.Slerp, InputReference reference = InputReference.Camera)
        {
            X = x;
            Z = z;
            Speed = speed;
            Type = type;
            Smooth = smooth;
            Reference = reference;
        }
    }

  
}
