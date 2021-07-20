using UnityEngine;

namespace GWLPXL.Movement.com
{
    public class InputRotate : ITick
    {
        Transform transform;
        InputRotateVars vars;
        public InputRotate(Transform transform, InputRotateVars vars)
        {
            this.transform = transform;
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

            Vector3 newMove = new Vector3(vars.X, 0, vars.Z);
            Vector3 translatedInput = newMove;
            switch (vars.Reference)
            {
                case InputReference.Camera:
                    translatedInput = MovementPrimary.TranslateToCamera(newMove);
                    break;
                case InputReference.Local:
                    translatedInput = MovementPrimary.TranslateToLocal(transform, newMove);
                    break;
            }

            Quaternion applied = MovementPrimary.GetLookRotation(transform, translatedInput, vars.Speed * vars.Multiplier, GetTickDuration(), vars.Smooth);
            transform.rotation = applied;
            vars.FaceDirection = applied;
        }


    }
    /// <summary>
    /// Input variables for a character
    /// </summary>
    [System.Serializable]
    public class InputRotateVars
    {
        public float Multiplier = 1;
        public float X;
        public float Z;
        public float Speed = 15;
        public RotateSmoothType Smooth = RotateSmoothType.Slerp;
        public InputReference Reference = InputReference.Camera;
        public Quaternion FaceDirection;
        public InputRotateVars(float x, float y, float z, float speed, RotateSmoothType smooth = RotateSmoothType.Slerp, InputReference reference = InputReference.Camera, float multiplier = 1)
        {
            X = x;
            Z = z;
            Speed = speed;
            Smooth = smooth;
            Reference = reference;
            Multiplier = multiplier;
        }
    }
}
