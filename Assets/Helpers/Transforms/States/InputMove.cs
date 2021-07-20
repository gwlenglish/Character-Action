using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.Movement.com
{
    [System.Serializable]
    public class InputMoveVars
    {
        public LayerMask Ground;
        public float X;
        public float Y;
        public float Z;
        public float Speed;
        public TransformMoveType Type;
        public InputReference Reference;
        public InputMoveVars(float x, float y, float z, float speed, TransformMoveType type,InputReference reference = InputReference.Camera)
        {
            X = x;
            Y = y;
            Z = z;
            Speed = speed;
            Type = type;
            Reference = reference;
        }
    }

    public class InputMove : ITick
    {
        Transform transform;
        InputMoveVars vars;
        public InputMove(Transform transform, InputMoveVars vars)
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

            Vector3 newMove = new Vector3(vars.X, vars.Y, vars.Z).normalized;
            Vector3 translted = newMove;

            switch (vars.Reference)
            {
                case InputReference.Camera:
                    translted = MovementPrimary.TranslateToCamera(newMove);
                    break;
                case InputReference.Local:
                    translted = MovementPrimary.TranslateToLocal(transform, newMove);
                    break;
            }

            bool hit = Detection.SimpleSpherecast(transform.position, vars.Speed * Time.fixedDeltaTime, translted, vars.Speed * Time.fixedDeltaTime, vars.Ground);
            if (hit == false)
            {
                switch (vars.Type)
                {
                    case TransformMoveType.AddPosition:
                        MovementPrimary.SimpleMove(transform, new SimpleMoveVars(translted, vars.Speed, Time.deltaTime, TransformMoveType.AddPosition));
                        break;
                    case TransformMoveType.MoveTowards:
                        MovementPrimary.SimpleMove(transform, new SimpleMoveVars(translted, vars.Speed, Time.deltaTime, TransformMoveType.MoveTowards));
                        break;
                    case TransformMoveType.Lerp:
                        MovementPrimary.SimpleMove(transform, new SimpleMoveVars(translted, vars.Speed, Time.deltaTime, TransformMoveType.Lerp));
                        break;
                }
            }
            RemoveTicker();
        }
    }



}
