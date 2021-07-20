using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GWLPXL.Movement.Character.com;
using GWLPXL.Movement.RB.com;
using GWLPXL.Movement.Character.CC.com;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.com
{

    /// <summary>
    /// wrapper class for commonly used movement functions 
    /// </summary>
    public static class MovementPrimary
    {


        #region character controller



        public static void AssignAnimatorParamInputsCC(PlayerControllerCC player, Animator animator)//to do, this needs to go
        {
            if (animator == null) return;

            animator.SetFloat(player.Controls.Controller.Locomotion.Animator.DirX, player.Controls.Controller.Locomotion.Movement.GlobalInput.x);
            animator.SetFloat(player.Controls.Controller.Locomotion.Animator.DirZ, player.Controls.Controller.Locomotion.Movement.GlobalInput.z);
            
            float speed = (player.Controls.Controller.Locomotion.Movement.GlobalInput * player.Controls.Controller.Locomotion.Movement.Speed).magnitude;
            animator.SetFloat(player.Controls.Controller.Locomotion.Animator.Speed, speed);
            animator.SetBool(player.Controls.Controller.Fall.Animator.AnimatorStateName, player.Controls.Controller.Fall.Falling.IsGrounded);

        }
       
        public static PlayerCacheStatesCC PlayerStateConstructionCC(CharacterController Controller, CharacterControllerSO Character, Animator Animator, System.Action<int> startsequencecallback)
        {
            PlayerCacheStatesCC states = new PlayerCacheStatesCC();
            states.Move = MovementPrimary.InputMoveCC(Controller, Character.PlayerCC);
            states.Falling = MovementPrimary.FallingCC(Controller, Character.PlayerCC.Controls.Controller.Fall.Falling);
            states.Rotate = MovementPrimary.InputRotate(Controller.transform, Character.PlayerCC.Controls.Controller.Rotate.Rotation);
            states.Inputs = new GatherPlayerStateInputs(Character.PlayerCC);
            states.AnimParams = new GatherAnimatorInputsCC(Character.PlayerCC, Animator);
            states.SequenceInputs = new GatherSequenceInputs(Character.PlayerCC, startsequencecallback);
            return states;
        }

        public static void RemoveStatesCC(PlayerCacheStatesCC states)
        {
            if (states == null) return;
            states.Move.RemoveTicker(); 
            states.Falling.RemoveTicker(); 
            states.Rotate.RemoveTicker(); 
            states.Inputs.RemoveTicker(); 
            states.AnimParams.RemoveTicker();
            states.SequenceInputs.RemoveTicker();
            states = null;
        }


        

        public static ActionCC ScriptedMoveCC(CharacterController controller, PlayerControllerCC playerC, Animator animator, ActionCCVars vars)
        {
            ActionCC state = new ActionCC(controller, playerC,animator, vars);
            return state;
        }
        public static FallingCC FallingCC(CharacterController controller, FallingVariablesCC vars)
        {
            FallingCC fall = new FallingCC(controller, vars);
            return fall;
        }
        public static void SimpleMoveCC(CharacterController controller, Vector3 newMove)
        {
            if ( controller == null || controller.enabled == false) return;
            controller.SimpleMove(newMove);
        }
        public static void MoveCC(CharacterController controller, Vector3 newMove)
        {
            if (controller == null || controller.enabled == false) return;
            controller.Move(newMove);
        }
      
        
        public static InputMoveCC InputMoveCC(CharacterController controller, PlayerControllerCC controls)
        {
            InputMoveCC move = new InputMoveCC(controller, controls);
            return move;
        }

        #endregion

        #region rigidbody
        public static void InputRotateRB(Rigidbody rigidbody, InputRotateVarsRB vars)
        {
            InputRotateRB rot = new InputRotateRB(rigidbody, vars);
        }
        public static InputRotate InputRotate(Transform transform, InputRotateVars vars)
        {
            InputRotate rot = new InputRotate(transform, vars);
            return rot;
        }
        public static void MoveRotation(Rigidbody rigidbody, RotateRBVars vars)
        {
            MoveRotationRB rot = new MoveRotationRB(rigidbody, vars);
        }
        public static void SetRotation(Rigidbody rigidbody, RotateRBVars vars)
        {
            SetRotationRB rot = new SetRotationRB(rigidbody, vars);
        }

        public static void InputMove(Rigidbody rigidbody, InputMoveVarsRB vars)
        {
            InputMoveRB move = new InputMoveRB(rigidbody, vars);
        }
        public static void Falling(Rigidbody rigidbody, Collider collider, FallingVariablesRB vars)
        {
            FallingRB fall = new FallingRB(rigidbody, collider, vars);
        }
        public static MoveLerpRB MoveLerp(Rigidbody rigidbody, MoveLerpVarsRB lerpvars, bool pingpong = false)
        {
            MoveLerpRB lerp = new MoveLerpRB(rigidbody, lerpvars, pingpong);
            return lerp;
        }
        public static AddForce AddForce(Rigidbody rigidbody, AddForceVars vars)
        {
            AddForce addforce = new AddForce(rigidbody, vars);
            return addforce;

        }

        public static SetVelocityRB SetVelocity(Rigidbody rigibody, Vector3 newVelocity, int ticks = 1)
        {
            SetVelocityRB velocity = new SetVelocityRB(rigibody, newVelocity, ticks);
            return velocity;
        }
        public static MovePositionRB MovePosition(Rigidbody rigibody, MovePositionVars vars)
        {
            MovePositionRB velocity = new MovePositionRB(rigibody, vars);
            return velocity;
        }
        public static AddVelocity AddVelocity(Rigidbody rigibody, VelocityVars vars)
        {
            AddVelocity velocity = new AddVelocity(rigibody, vars);
            return velocity;
        }
        #endregion

        #region transforms
        public static Vector3 GetTranslatedRotateDirection(Transform transform, float angle, AngleAxis angleaxis)
        {
            Vector3 axis = Vector3.zero;

            switch (angleaxis)
            {
                case AngleAxis.LocalUp:
                    axis = transform.up;
                    break;
                case AngleAxis.LocalDown:
                    axis = -transform.up;
                    break;
                case AngleAxis.LocalFoward:
                    axis = transform.forward;
                    break;
                case AngleAxis.LocalLeft:
                    axis = -transform.right;
                    break;
                case AngleAxis.LocalRight:
                    axis = transform.right;
                    break;
                case AngleAxis.LocalBack:
                    axis = -transform.forward;
                    break;
                case AngleAxis.WorldBack:
                    axis = -Vector3.forward;
                    break;
                case AngleAxis.WorldDown:
                    axis = -Vector3.up;
                    break;
                case AngleAxis.WorldFoward:
                    axis = Vector3.forward;
                    break;
                case AngleAxis.WorldLeft:
                    axis = -Vector3.right;
                    break;
                case AngleAxis.WorldRight:
                    axis = Vector3.right;
                    break;
                case AngleAxis.WorldUp:
                    axis = Vector3.up;
                    break;
            }

            Vector3 lockeddir = Quaternion.AngleAxis(angle, axis) * transform.forward;
            return lockeddir;
        }
        public static Rotate Rotate(Transform transform, RotateVars vars)
        {
            Rotate rotate = new Rotate(transform, vars);
            return rotate;
        }
        public static void Leap(Transform transform, LeapVars vars)
        {
            Leap leap = new Leap(transform, vars);
        }
        public static void InputMove(Transform transform, InputMoveVars vars)
        {
            InputMove move = new InputMove(transform, vars);
        }
        public static SimpleMove SimpleMove(Transform transform, SimpleMoveVars vars)
        {
            SimpleMove simplemove = new SimpleMove(transform, vars);
            return simplemove;
        }
        public static SimpleMove SimpleMove(Transform transform, SimpleMoveVars vars, Action<SimpleMove> OnComplete)
        {
            SimpleMove simplemove = SimpleMove(transform, vars);
            simplemove.OnComplete += OnComplete;
            return simplemove;
        }
        public static MoveLerp MoveLerp(Transform transform, MoveLerpVars lerpvars, bool pingpong = false)
        {
            MoveLerp lerp = new MoveLerp(transform, lerpvars, pingpong);
            return lerp;
        }
        public static void Falling(Collider collider, FallingVariables vars)
        {
            Falling fall = new Falling(collider, vars);
        }
        #endregion

        #region misc
       
        public static Quaternion GetLookRotation(Transform transform, Vector3 input, float speed, float dt, RotateSmoothType type)
        {
            Quaternion rot = Quaternion.LookRotation(transform.transform.forward + input, Vector3.up);
            Quaternion rotatet = Quaternion.RotateTowards(transform.rotation, rot, speed);
            Quaternion slerped = Quaternion.Slerp(transform.rotation, rotatet, dt * speed);//gives more control
            Quaternion applied = Quaternion.identity;

            switch (type)
            {
                case RotateSmoothType.Instant:
                    applied = rotatet;
                    break;
                case RotateSmoothType.Slerp:
                    applied = slerped;
                    break;
            }

            return applied;
        }
        public static Quaternion GetLookRotation(Rigidbody rigidbody, Vector3 input, float speed, float dt, RotateSmoothType type)
        {
            Quaternion rot = Quaternion.LookRotation(rigidbody.transform.forward + input, Vector3.up);
            Quaternion rotatet = Quaternion.RotateTowards(rigidbody.rotation, rot, speed);
            Quaternion slerped = Quaternion.Slerp(rigidbody.rotation, rotatet, dt * speed);//gives more control
            Quaternion applied = Quaternion.identity;

            switch (type)
            {
                case RotateSmoothType.Instant:
                    applied = rotatet;
                    break;
                case RotateSmoothType.Slerp:
                    applied = slerped;
                    break;
            }

            return applied;
        }
        public static Vector3 TranslateToCamera(Vector3 newMove)
        {
            Vector3 translatedInput;
            //we take into account the camera's rotation
            Vector3 movedirH = Camera.main.transform.right * newMove.x;
            movedirH.y = 0;//we 0 out y otherwise we'll face the camera like a selfie
            Vector3 movedirV = Camera.main.transform.forward * newMove.z;
            movedirV.y = 0;
            translatedInput = movedirH + movedirV;
            return translatedInput;
        }
        public static Vector3 TranslateToLocal(Transform local, Vector3 newMove)
        {
            Vector3 translatedInput = local.TransformDirection(newMove);
            return translatedInput;
        }
        public static void Depentrate(Collider collider, StickVars vars)
        {
            Depentrate(collider as CapsuleCollider, vars);
        }
       
      
     

        public static void StickToGround(Collider collider, StickVars vars)
        {
            StickToGround stick = new StickToGround(collider, vars);
        }

        public static bool SphereStickToCollider(CapsuleCollider capsule, StickVars vars)
        {
            bool stick = TryStickCapsule(capsule, vars);


            return stick;
        }
        public static bool SimpleStickToCollider(Collider sticker, StickVars vars, float step = .1f)
        {
  
            float startX = sticker.bounds.min.x;
            float startZ = sticker.bounds.center.z;
            float staryY = sticker.bounds.min.y;
            int increment = Mathf.RoundToInt(sticker.bounds.size.x / step);//assumes uniform size, like capsules, sphere, boxes, primitives
            Vector3 runningNewStep = new Vector3(startX, staryY, startZ) + vars.Direction;
            runningNewStep = sticker.bounds.min;
            for (int i = 0; i < increment; i++)
            {
                runningNewStep.x += step;//running steps

                //try x
                bool stick = TryStick(sticker, vars, runningNewStep);
                if (stick) return true;
                Vector3 negx = runningNewStep;//check opposite side
                negx.x *= -1;
                stick = TryStick(sticker, vars, negx);
                if (stick) return true;

                //try z
                Vector3 z = runningNewStep;
                z.z *= step * i + step;
                stick = TryStick(sticker, vars, z);
                if (stick) return true;
                Vector3 negz = z;
                z.z *= -1;
                stick = TryStick(sticker, vars, negz);
                if (stick) return true;

            }

            return false;
        }
        public static void SimpleStickToCollider2D(Collider2D collider, StickVars vars, float step = .1f)
        {

            float startX = collider.bounds.min.x;
            float staryY = collider.bounds.min.y;
            int increment = Mathf.RoundToInt(collider.bounds.size.x / step);
            Vector2 newStep = new Vector2(startX, staryY);
            for (int i = 0; i < increment; i++)
            {
                newStep.x += step;
                bool hit = TryStick(collider, vars, newStep);
                if (hit) return;
                Vector3 negx = newStep;//check opposite side
                negx.x *= -1;
                hit = TryStick(collider, vars, negx);
                if (hit) return;
            }
        }


        public static void Depentrate(CapsuleCollider capsule, StickVars vars)
        {

            TryStickCapsule(capsule, vars);
        }

        static Vector3 lasthit;
        static float depentrationspeed = 25;
        public static void TryClimbSlope(CapsuleCollider capsule, LayerMask ground, float RayLength = .6f, float MaxSlopeAngle = 45)
        {

            Vector3 ray1start = capsule.transform.position + -capsule.transform.up * capsule.height * .4f;
            RaycastHit hit = Detection.SimpleRaycastHit(ray1start, capsule.transform.forward, RayLength, ground);
            if (hit.collider != null)
            {
                Vector3 forward = Vector3.Cross(hit.normal, -capsule.transform.right);
                float angle = Vector3.Angle(hit.normal, capsule.transform.forward);

                if (Mathf.Abs(angle - 90) <= MaxSlopeAngle && Mathf.Approximately(angle, 90) == false)//lower then max climb and not standing straight up
                {

                    // Rb.AddForce(forward * Strength);
                    SimpleMove(capsule.transform, new SimpleMoveVars(forward, forward.magnitude, Time.deltaTime));
                }

            }

        }

        public static void TryFixOverlap(CapsuleCollider capsule, LayerMask layermask, Vector3 veldirection, float fixSpeed)
        {
            veldirection.Normalize();
            float radius = capsule.radius * .4f;
            Ray ray2 = new Ray(capsule.transform.position + new Vector3(0, capsule.height / 2, 0), Vector3.down);
            Vector3 pretendcapsulemax = capsule.bounds.max + veldirection;
            Vector3 pretendcapsulemin = capsule.bounds.min + veldirection;
            //maybe now try spherecast
            Collider[] colls = Physics.OverlapCapsule(pretendcapsulemin,pretendcapsulemax, radius, layermask);
            Debug.Log("OVerlap " + colls.Length);
            Collider closet = null;
            float closest = Mathf.Infinity;
            Vector3 direction = Vector3.zero;
            for (int i = 0; i < colls.Length; i++)
            {
                direction = colls[i].ClosestPointOnBounds(capsule.transform.position) - capsule.transform.position;
                float sqrdmag = direction.sqrMagnitude;
                if (sqrdmag < closest)
                {
                    closet = colls[i];
                    closest = sqrdmag;
                }
            }
            if (closet != null)
            {
                Vector3 scaledmin = Vector3.Scale(capsule.bounds.min, -veldirection);
                Vector3 scaledgoal = Vector3.Scale(closet.bounds.max, -veldirection);
                direction = capsule.bounds.min - closet.bounds.max;
                direction = scaledmin - scaledgoal;
                SimpleMove(capsule.transform, new SimpleMoveVars(-direction, direction.magnitude * fixSpeed, Time.deltaTime));
            }


        }
        public static bool TryStickCapsule(CapsuleCollider capsule, StickVars vars)
        {

            float radius = capsule.radius * .9f;
            Ray ray2 = new Ray(capsule.transform.position + new Vector3(0, capsule.height / 2, 0), Vector3.down);
            //maybe now try spherecast
            RaycastHit hit = Detection.SimpleSpherecastHit(capsule.bounds.max, capsule.radius * .9f, vars.Direction, capsule.height *.9f, vars.LayerMask);
            if (hit.collider != null)
            {
                Debug.Log("Hit at " + hit.point);
                Vector3 newhit = new Vector3((float)Math.Round(hit.point.x, 2), (float)Math.Round(hit.point.y, 2), (float)Math.Round(hit.point.z, 2));

                Vector3 currentgroundpoint = capsule.bounds.min;
                Vector3 currentpivot = capsule.transform.position;
                Vector3 pivotgrounddifference = currentpivot - currentgroundpoint;

                DebugHelpers.DebugWireSphere(hit.point, capsule.radius * .9f, Color.red);
                DebugHelpers.DebugLine(hit.point, capsule.bounds.min, Color.red);

           

                Vector3 scaledhit = Vector3.Scale(hit.point, vars.Direction);
                Vector3 scaledposition = Vector3.Scale(capsule.bounds.min, capsule.transform.TransformDirection(-vars.Direction));

                float dot = Vector3.Dot(scaledhit, scaledposition);
                if (dot > 0)
                {
                    Debug.Log("Above");
                }
                else if (dot < 0)
                {
                    Debug.Log("below");
                    Vector3 direction = scaledhit - scaledposition;
                    Debug.DrawLine(scaledhit, scaledposition);
                    SimpleMove(capsule.transform, new SimpleMoveVars(direction, direction.magnitude * depentrationspeed, Time.deltaTime));
                    lasthit = direction;
                }

               






            }
            return true;
        }
        static bool TryStick(Collider sticker, StickVars vars, Vector3 runningNewStep)
        {
            Vector3 posX = runningNewStep;
            RaycastHit hit = Detection.SimpleRaycastHit(runningNewStep, vars.Direction, vars.MaxStickDistance, vars.LayerMask);
            if (hit.collider != null)
            {
                float distance = Mathf.Abs(hit.point.y - runningNewStep.y);
                if (distance > vars.MinStickDistance)
                {
                    SimpleMove(sticker.transform, new SimpleMoveVars(vars.Direction, vars.StickStrength * Time.deltaTime, Time.deltaTime));
                    DebugHelpers.DebugLine(runningNewStep, hit.point, Color.red);
                    return true;
                }

            }
            return false;
        }
        static bool TryStick(Collider2D sticker, StickVars vars, Vector3 runningNewStep)
        {
            Vector3 posX = runningNewStep;
            RaycastHit2D hit = Detection.SimpleRaycastHit2D(runningNewStep, vars.Direction, vars.MaxStickDistance, vars.LayerMask);
            if (hit.collider != null)
            {
                float distance = Mathf.Abs(hit.point.y - runningNewStep.y);
                if (distance > vars.MinStickDistance)
                {

                    SimpleMove(sticker.transform, new SimpleMoveVars(vars.Direction, vars.StickStrength * Time.deltaTime, Time.deltaTime));
                    DebugHelpers.DebugLine(runningNewStep, hit.point, Color.red);
                    return true;
                }

            }
            return false;
        }

        #endregion
    }


   
    public enum InputReference
    {
        World = 0,
        Camera = 1,
        Local = 2
    }
    public enum RotateSmoothType
    {
        Instant = 0,
        Slerp = 1
    }
   
    public enum AngleAxis
    {
        LocalUp = 0,
        LocalFoward = 1,
        LocalRight = 2,
        LocalLeft = 3,
        LocalDown = 4,
        LocalBack = 5,

        WorldUp = 10,
        WorldFoward = 11,
        WorldRight = 12,
        WorldLeft = 13,
        WorldDown = 14,
        WorldBack = 15
    }
    public enum TransformMoveType
    {
        SetPosition = 0,
        AddPosition = 1,
        MoveTowards = 2,
        Lerp = 3
    }
    public enum PhysicsType
    {
        Physics3D = 0,
        Physics2D = 1
    }

   

 

    [System.Serializable]
    public struct StickVars
    {
        public LayerMask LayerMask;
        public Vector3 Direction;
        [Tooltip("Distance to check for alignment")]
        public float MaxStickDistance;
        [Tooltip("If greater than this number, will perform the logic for alignment.")]
        public float MinStickDistance;
        [Tooltip("How quickly to align to collider")]
        public float StickStrength;

        public StickVars(LayerMask mask, Vector3 dir, float distance, float delta, float speed)
        {
            LayerMask = mask;
            Direction = dir;
            MaxStickDistance = distance;
            MinStickDistance = delta;
            StickStrength = speed;
        }
    }



}