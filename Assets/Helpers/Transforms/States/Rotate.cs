using UnityEngine;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.com
{
    public class Rotate : ITick
    {
        RotateVars vars;
        Transform transform;
        float runningAngle = 0;
        float timer = 0;
        Quaternion start;
        float correcttimer = 0;
        float correctrate = .1f;
        public Rotate(Transform transform, RotateVars vars)
        {
            this.vars = vars;
            this.transform = transform;
            this.start = transform.rotation;
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
            transform.rotation = start;
            TickManager.RemoveTicker(this);
        }

        public void Tick()
        {
            if (transform == null)
            {
                RemoveTicker();
                return;
            }
            float dt = GetTickDuration();
            timer += dt;

            if (timer <= vars.Duration)
            {
                runningAngle += vars.AnglesPerSecond * dt;
                System.Math.Round((float)runningAngle, 2);
                Quaternion rot = Quaternion.AngleAxis(runningAngle, vars.RotateAxis);
                Quaternion towards = Quaternion.RotateTowards(transform.rotation, rot, vars.AnglesPerSecond * dt);
                transform.rotation = rot;
            }
            else
            {
                if (vars.Forever)
                {
                    runningAngle += vars.AnglesPerSecond * dt;
                    Quaternion rot = Quaternion.AngleAxis(runningAngle, vars.RotateAxis);
                    Quaternion towards = Quaternion.RotateTowards(transform.rotation, rot, vars.AnglesPerSecond * dt);
                    transform.rotation = rot;
                }
                else
                {
                    correcttimer += dt;
                    Quaternion rot = Quaternion.Slerp(transform.rotation, start, correcttimer / correctrate);
                    transform.rotation = rot;
                    if (correcttimer >= correctrate)
                    {
                        RemoveTicker();
                    }
    
                }
            }

   
        }
    }

    [System.Serializable]
    public struct RotateVars
    {
        public Vector3 RotateAxis;
        public float AnglesPerSecond;
        [Tooltip("Set greater to 0 for limited time.")]
        public float Duration;
        public bool Forever;
        public RotateVars(Vector3 axis, float anglespersecond, float duration, bool forever = false)
        {
            RotateAxis = axis;
            AnglesPerSecond = anglespersecond;
            Duration = duration;
            Forever = forever;
        }
    }


    public class RotateRB : ITick
    {
        RotateVars vars;
        Rigidbody rigidbody;
        float runningAngle = 0;
        float timer = 0;
        public RotateRB(Rigidbody rigidbody, RotateVars vars)
        {
            this.vars = vars;
            this.rigidbody = rigidbody;
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
            if (rigidbody == null)
            {
                RemoveTicker();
                return;
            }
            float dt = GetTickDuration();
            if (vars.Duration > 0)
            {
                timer += dt;
                if (timer >= vars.Duration)
                {
                    vars.Duration = 0;
                    timer = 0;
                }
                else
                {
                    runningAngle += vars.AnglesPerSecond * dt;
                    Quaternion rot = Quaternion.AngleAxis(runningAngle, vars.RotateAxis);
                    rigidbody.MoveRotation(rot);
                }
            }
            else if (vars.Duration < 0)
            {
                runningAngle += vars.AnglesPerSecond * dt;
                Quaternion rot = Quaternion.AngleAxis(runningAngle, vars.RotateAxis);
                rigidbody.MoveRotation(rot);
            }

        }
    }

}