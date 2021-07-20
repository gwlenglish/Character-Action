using UnityEngine;
/// <summary>
/// goal, most re-used methods so dont need to re-write things
/// </summary>
namespace GWLPXL.Movement.com
{
    public static class DebugHelpers
    {
        public static void DebugWireSphere(Vector3 center, float radius, Color color)
        {
#if UNITY_EDITOR
            //UnityEditor.Handles.SphereHandleCap(0, center, Quaternion.identity, radius, EventType.Repaint);
           // Gizmos.color = color;
            //Gizmos.DrawWireSphere(center, radius);
#endif
        }
        public static void DebugLine(Vector3 start, Vector3 end, Color color)
        {
#if UNITY_EDITOR
           // UnityEditor.Handles.DrawLine(start, end);
            Debug.DrawLine(start, end, color);
#endif
        }

        
        public static void DrawBezier(Vector3 start, Vector3 end, Color color, Texture2D text, float thickness)
        {
#if UNITY_EDITOR
            Vector3 p1 = start;
            Vector3 p2 = end;
            Color c = color;
            Texture2D tet = text;
            float thick = thickness;
            UnityEditor.Handles.DrawBezier(p1, p2, p1, p2, color, tet, thick);

#endif
        }
    }

}