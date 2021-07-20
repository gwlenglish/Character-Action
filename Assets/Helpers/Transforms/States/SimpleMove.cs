using UnityEngine;
using System;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.com
{
   
    public class SimpleMove : ITick
    {
        public Action<SimpleMove> OnComplete;
        SimpleMoveVars vars;
        Transform transform;
        float timer = 0;
        public SimpleMove(Transform transform, SimpleMoveVars vars)
        {
            this.transform = transform;
            this.vars = vars;
            this.vars.Direction = vars.Direction.normalized;
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
            OnComplete?.Invoke(this);
        }

        public void Tick()
        {
            if (transform == null)
            {
                RemoveTicker();
                return;
            }
            timer += GetTickDuration();
            Vector3 goal = transform.position + (vars.Direction.normalized * vars.Speed);
            switch (vars.Type)
            {
                case TransformMoveType.SetPosition:
                    transform.position = vars.Direction;
                    break;
                case TransformMoveType.AddPosition:
                    transform.position += vars.Direction.normalized * vars.Speed * GetTickDuration();
                    break;
                case TransformMoveType.MoveTowards:
                    transform.position = Vector3.MoveTowards(transform.position,  goal, GetTickDuration() * Mathf.Abs(vars.Speed));
                    break;
                case TransformMoveType.Lerp:
                    float percent = timer / vars.Duration;
                    if (vars.Curve != null)
                    {
                        percent = vars.Curve.Evaluate(timer/vars.Duration);
                    }
                    transform.position = Vector3.Lerp(transform.position, goal, percent);
                    break;
            }

            if (timer >= vars.Duration)
            {
                RemoveTicker();
            }
        }
    }

    [System.Serializable]
    public struct SimpleMoveVars
    {
        public Vector3 Direction;
        public float Speed;
        public float Duration;
        public TransformMoveType Type;
        [Tooltip("For lerp only")]
        public AnimationCurve Curve;
        public SimpleMoveVars(Vector3 direction, float speed, float duration, TransformMoveType type = TransformMoveType.AddPosition, AnimationCurve curve = null)
        {
            Direction = direction;
            Speed = speed;
            Duration = duration;
            Type = type;
            Curve = curve;
        }
    }

}