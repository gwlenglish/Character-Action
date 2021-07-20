using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.Movement.com
{


    [System.Serializable]
    public struct LeapVars
    {
        [HideInInspector]
        public LayerMask StopLayers;//hidden because not working as intended at the moment
        public Vector3 Direction;
        [Header("Horizontal Movement")]
        public AnimationCurve HorizontalCurve;
        public float Distance;
        public float Duration;
        [Header("Vertical Movement")]
        public AnimationCurve UpwardCurve;
        public AnimationCurve FallCurve;
        public float UpwardForce;
        [Range(0, 1f)]
        public float TimeToApex;

        public LeapVars(Vector3 direction, float distance, float duration, float upwardForce, float timeToApex, LayerMask stoppingLayers, AnimationCurve horizontalCurve, AnimationCurve upward, AnimationCurve fall)
        {
            Direction = direction;
            Distance = distance;
            Duration = duration;
            UpwardForce = upwardForce;
            TimeToApex = timeToApex;
            StopLayers = stoppingLayers;
            HorizontalCurve = horizontalCurve;
            UpwardCurve = upward;
            FallCurve = fall;
        }

    }


    public class Leap : ITick
    {
        LeapVars vars;
        Transform knocked;
        Vector3 hitdirection;
        Rigidbody rb = null;
        float timer;
        float falltimer;
        float apextimer;
        Vector3 start;
        float goaly = 0;
        bool active;
        bool stophorizontal;
        Vector3 previous;
        Vector3 lerp;

        bool stopupwards;
        float distance;
        public Leap(Transform knocked, LeapVars vars)
        {
            this.vars = vars;
            this.knocked = knocked;
            this.distance = vars.Distance;
            this.hitdirection = vars.Direction;
            start = knocked.position;
            goaly = knocked.position.y * vars.UpwardForce;
            lerp = start;
            AddTicker();
        }






        public void AddTicker()
        {
            TickManager.AddTicker(this);
        }

        public void Tick()
        {

            timer += GetTickDuration();
            apextimer += GetTickDuration();

            #region known issues with uneven ground, ie stairs, failed attempts to fix
            if (stophorizontal == false)
            {
                Ray ray = new Ray(knocked.position, hitdirection);
                RaycastHit hit;
                //need sphere cast
                Physics.SphereCast(previous, .01f, knocked.TransformDirection(hitdirection * vars.Distance * vars.HorizontalCurve.Evaluate((timer + GetTickDuration()) / vars.Duration)), out hit, vars.StopLayers);
                if (hit.collider != null)
                {
                    //stophorizontal = true;
                    Debug.Log("Stopped");
                }

            }

            if (stopupwards == false)
            {
                float currenty = knocked.position.y;
                Ray ray = new Ray(knocked.position, hitdirection);
                RaycastHit hit;
                //sphere cast again
                Physics.SphereCast(previous, .01f, knocked.TransformDirection(new Vector3(0, currenty, 0) * vars.Distance * vars.UpwardCurve.Evaluate((timer + GetTickDuration()) / vars.Duration)), out hit, vars.StopLayers);
                if (hit.collider != null)
                {
                    stopupwards = true;
                    Debug.Log("Stopped");
                }
            }
            #endregion

            if (stophorizontal == false)
            {
                lerp = Vector3.Lerp(start, start + (hitdirection * distance), vars.HorizontalCurve.Evaluate(timer / vars.Duration));
            }
            else
            {
                lerp = previous;

            }
            lerp.y = 0;




            float lerpy = start.y;
            if (apextimer <= (vars.Duration * vars.TimeToApex))
            {
                //upwards
                if (stopupwards == false)
                {
                    lerpy = Mathf.Lerp(start.y, goaly, apextimer / vars.UpwardCurve.Evaluate(vars.Duration / vars.TimeToApex));
                }

            }
            else
            {
                falltimer += GetTickDuration();
                //downwards
                lerpy = Mathf.Lerp(goaly, start.y, (falltimer / vars.FallCurve.Evaluate(1 - vars.TimeToApex)));
            }



            Vector3 next = lerp + new Vector3(0, lerpy, 0);
            knocked.transform.position = next;
           
            previous = lerp;
            if (timer >= vars.Duration)
            {
                RemoveTicker();
            }
        }

        public float GetTickDuration() => Time.deltaTime;


        public void RemoveTicker()
        {
            TickManager.RemoveTicker(this);
        }


    }
}