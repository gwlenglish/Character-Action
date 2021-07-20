using System;
using UnityEngine;
namespace GWLPXL.Movement.com
{
 

    [System.Serializable]
    public class MoveLerpVars
    {
        public LayerMask Blocking;
        public Vector3 Direction;
        public float Distance;
        public float Duration;
        public float WaitDuration;
        public AnimationCurve MoveCurve;
        public InputReference Reference;
        public MoveLerpVars(Vector3 dir, float dist, float duration, float waitduration, AnimationCurve curve, InputReference refe = InputReference.Local)
        {
            Direction = dir;
            Distance = dist;
            Duration = duration;
            WaitDuration = waitduration;
            MoveCurve = curve;
            Reference = refe;
        }
    }


    public class MoveLerp : ITick
    {
        public Action<MoveLerp> OnComplete;
        public Action<MoveLerp> OnDestinationReached;
        public Action OnNewDestinationStarted;
        public MoveLerpVars Vars;
        Transform transform;
        float timer = 0;
        float waittimer = 0;
        float directionMultipler = 1;
        bool wait;
        Vector3 start;
        Vector3 end;
        bool pingpong;
        public MoveLerp(Transform transform, MoveLerpVars vars, bool pingPong = true)
        {
            this.transform = transform;
            this.Vars = vars;
            this.pingpong = pingPong;
            start = transform.position;
            Vector3 dir = vars.Direction;
            switch (vars.Reference)
            {
                case InputReference.Local:
                    dir = MovementPrimary.TranslateToLocal(transform, vars.Direction);
                    break;
                case InputReference.Camera:
                    dir = MovementPrimary.TranslateToCamera(vars.Direction);
                    break;
            }

            end = start + dir * directionMultipler * Vars.Distance;
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
            OnComplete?.Invoke(this);
            TickManager.RemoveTicker(this);
        }

        public void Tick()
        {
            if (transform == null)
            {
                RemoveTicker();
                return;
            }
            timer += GetTickDuration();
            if (timer >= Vars.Duration)
            {
                if (waittimer == 0)
                {
                    OnDestinationReached?.Invoke(this);
                }
                waittimer += GetTickDuration();
                wait = true;
                if (waittimer >= Vars.WaitDuration)
                {
                    directionMultipler *= -1;
                    timer = 0;
                    waittimer = 0;
                    wait = false;
                    start = transform.position;
                    end = start + Vars.Direction * directionMultipler * Vars.Distance;
                    OnNewDestinationStarted?.Invoke();
                    if (pingpong == false)
                    {
                        RemoveTicker();
                    }

                }

            }

            if (wait) return;
            float percent = timer / Vars.Duration;
            if (Vars.MoveCurve != null)
            {
                percent = Vars.MoveCurve.Evaluate(percent);
            }
            // Debug.Log(percent);
            Vector3 lerp = Vector3.Lerp(start, end, percent);

            transform.position = lerp;
          

        }
    }


}