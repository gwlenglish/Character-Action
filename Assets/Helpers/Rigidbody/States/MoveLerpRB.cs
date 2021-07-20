
using GWLPXL.Movement.com;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace GWLPXL.Movement.RB.com
{
    [System.Serializable]
    public class MoveLerpVarsRB
    {
        public Vector3 Direction;
        public float Distance;
        public float Duration;
        public float WaitDuration;
        public AnimationCurve MoveCurve;
        public MoveLerpVarsRB(Vector3 dir, float distance, float duration, float waitduration, AnimationCurve curve)
        {
            Direction = dir;
            Distance = distance;
            Duration = duration;
            WaitDuration = waitduration;
            MoveCurve = curve;
        }
    }
    public class MoveLerpRB : ITick
    {
        List<Rigidbody> riders;
        List<CharacterController> charControllers;

        public Action OnDestinationReached;
        public Action OnNewDestinationStarted;
        public MoveLerpVarsRB Vars;
        Rigidbody rb;
        float timer = 0;
        float waittimer = 0;
        float directionMultipler = 1;
        bool wait;
        Vector3 start;
        Vector3 end;
        bool pingpong;
        float fixedtimer = 0;

        public void AddRider(Rigidbody rb)
        {
            if (riders.Contains(rb) == false) riders.Add(rb);
        }
        public void AddRider(CharacterController controller)
        {
            if (charControllers.Contains(controller) == false) charControllers.Add(controller);
        }
        public void RemoveRider(Rigidbody rb)
        {
            if (riders.Contains(rb) == true) riders.Remove(rb);
        }
        public void RemoveRider(CharacterController controller)
        {
            if (charControllers.Contains(controller) == true) charControllers.Remove(controller);
        }
        public MoveLerpRB(Rigidbody rigidbody, MoveLerpVarsRB vars, bool pingPong = false)
        {
            riders = new List<Rigidbody>();
            charControllers = new List<CharacterController>();
            this.rb = rigidbody;
            this.Vars = vars;
            this.pingpong = pingPong;
            start = rigidbody.position;
            end = start + vars.Direction * directionMultipler * Vars.Distance;
            AddTicker();


        }
        public void AddTicker()
        {
            OnDestinationReached += ZeroutRiders;
            TickManager.AddTicker(this as ITick);
        }

        public float GetTickDuration()
        {
            return Time.deltaTime;
        }


        public void RemoveTicker()
        {
            OnDestinationReached -= ZeroutRiders;
            TickManager.RemoveTicker(this as ITick);
        }

        public void Tick()
        {
            
            if (rb == null)
            {
                RemoveTicker();
                return;
            }
            fixedtimer += Time.deltaTime;
            if (fixedtimer >= Time.fixedDeltaTime)
            {
                fixedtimer = 0;
            }
            timer += GetTickDuration();
            if (timer >= Vars.Duration)
            {
                if (waittimer == 0)
                {
                    OnDestinationReached?.Invoke();
                }
                waittimer += GetTickDuration();
                wait = true;
                if (waittimer >= Vars.WaitDuration)
                {
                    directionMultipler *= -1;
                    timer = 0;
                    waittimer = 0;
                    wait = false;
                    start = rb.position;
                    end = start + Vars.Direction * directionMultipler * Vars.Distance;
                    OnNewDestinationStarted?.Invoke();
                    if (pingpong == false)
                    {
                        RemoveTicker();
                    }

                }

            }
            float curvePosition = Vars.MoveCurve.Evaluate(timer / Vars.Duration);
            Vector3 lerp = Vector3.Lerp(start, end, curvePosition);
            MovePositionRB lerpmove = new MovePositionRB(rb, new MovePositionVars(lerp));

            Vector3 deltaPosition = lerp - rb.transform.position;
            for (int i = 0; i < charControllers.Count; i++)
            {
                MovementPrimary.MoveCC(charControllers[i], deltaPosition);
            }

            rb.transform.position = lerp;

            if (wait == false)//means we are moving
            {
                for (int i = 0; i < riders.Count; i++)
                {
                    Debug.Log("Rider " + riders[i].name);

                    SetVelocityRB vel = new SetVelocityRB(rb, Vector3.Scale(rb.velocity, Vars.Direction));
                    //riders[i].velocity = rb.velocity;

                     riders[i].velocity = Vector3.Scale(rb.velocity, Vars.Direction);

                }
            }



           
        }

        void ZeroutRiders()
        {//try later
            for (int i = 0; i < riders.Count; i++)
            { 
                RaycastHit hit = Detection.SimpleRaycastHit(riders[i].GetComponent<Collider>().bounds.min, rb.transform.position - riders[i].transform.position, 10);
                Vector3 dir = hit.point - riders[i].transform.position;
                SimpleMove move = new SimpleMove(riders[i].transform, new SimpleMoveVars(dir, 1, Time.deltaTime, TransformMoveType.AddPosition));
                Debug.Log("Rider " + riders[i].name);
                SetVelocityRB vel = new SetVelocityRB(rb, Vector3.zero);
                riders[i].velocity = Vector3.zero;
            }
        }

    }


}