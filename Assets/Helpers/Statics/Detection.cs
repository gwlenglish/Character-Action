using UnityEngine;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.com
{
    /// <summary>
    /// wrapper class for commonly used detection options in unity
    /// </summary>
    public static class Detection
    {
        public static bool SimpleCapsuleCast(Vector3 p0, Vector3 p1, float radius, Vector3 dir, float distance, LayerMask layer)
        {
            return Physics.CapsuleCast(p0, p1, radius, dir, distance, layer);
          

        }
        public static bool SimpleCapsuleCast(Vector3 p0, Vector3 p1, float radius, Vector3 dir, float distance, LayerMask layer, QueryTriggerInteraction q = QueryTriggerInteraction.UseGlobal)
        {
            return Physics.CapsuleCast(p0, p1, radius, dir, distance, layer);
            Collider[] coll = Physics.OverlapCapsule(p0, p1, radius, layer, q);
            for (int i = 0; i < coll.Length; i++)
            {
                Debug.Log(coll[i]);
            }
            return coll.Length > 0;

        }
        public static bool SimpleSpherecast(Vector3 pos, float radius, Vector3 direction, float length, LayerMask layermask)
        {
            RaycastHit hit;
            return Physics.SphereCast(pos, radius, direction, out hit, length, layermask);
        }
        public static RaycastHit SimpleSpherecastHit(Vector3 pos, float radius, Vector3 direction, float length, LayerMask layermask)
        {
            RaycastHit hit;
            Physics.SphereCast(pos, radius, direction, out hit, length, layermask);
            return hit;
        }

        public static bool SimpleRaycast(Ray ray, float length, LayerMask layermask)
        {
            return Physics.Raycast(ray, length, layermask);
        }
        public static bool SimpleRaycast(Vector3 startpos, Vector3 direction, float length, LayerMask layermask)
        {
            Ray ray = new Ray(startpos, direction);
            return SimpleRaycast(ray, length, layermask);
        }
        public static RaycastHit SimpleRaycastHit(Vector3 startpos, Vector3 direction, float length, LayerMask layermask)
        {
            Ray ray = new Ray(startpos, direction);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, length, layermask);
            return hit;
        }
        public static RaycastHit SimpleRaycastHit(Vector3 startpos, Vector3 direction, float length)
        {
            Ray ray = new Ray(startpos, direction);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, length);
            return hit;
        }
        public static bool SimpleRaycast2D(Vector2 startpos, Vector2 direction, float distance, LayerMask layermask)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(startpos, direction, distance, layermask);
            return hit.collider != null;
        }
        public static RaycastHit2D SimpleRaycastHit2D(Vector2 startpos, Vector2 direction, float distance, LayerMask layermask)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(startpos, direction, distance, layermask);
            return hit;
        }

        public static bool HasLineOfSight(Transform viewer, Transform viewee, LayerMask blockingLayers, float maxRange, int verticalRays = 3, float verticalRayStep = 1)
        {
            Vector3 direction = viewee.transform.position - viewer.transform.position;
            direction.Normalize();
            float verticalstart = 0;
            Ray[] rays = new Ray[verticalRays];
            for (int i = 0; i < rays.Length; i++)
            {
                rays[i] = new Ray(viewer.transform.position + Vector3.up * verticalstart, direction);//refactor this out to be a direction
                verticalstart += verticalRayStep;
            }
            for (int i = 0; i < rays.Length; i++)
            {
                if (Detection.SimpleRaycast(rays[i], maxRange, blockingLayers))
                {
                    return true;
                }
            }

            return false;

        }

        public static bool HasSight(Transform user, Vector3 userforward, Transform target, float forwardSightAngle, PhysicsType type = PhysicsType.Physics3D)
        {
            float angle = 0;
            Vector3 targetDir = target.position - user.transform.position;
            targetDir = targetDir.normalized;

            switch (type)
            {
                case PhysicsType.Physics3D:
                    float dot = Vector3.Dot(targetDir, userforward);
                    angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                    break;
                case PhysicsType.Physics2D:
                    angle = Vector2.Angle(userforward, target.transform.position);
                    break;
            }

            return angle <= forwardSightAngle;
        }
    }

}