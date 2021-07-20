using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace GWLPXL.Helpers.com
{


    [System.Serializable]
    public class PhysicsCallbacks
    {
        public System.Action<Collider> OnTriggerEnter;
        public System.Action<Collider> OnTriggerExit;
        public System.Action<Collider> OnTriggerStay;
        public System.Action<Collision> OnCollisionEnter;
        public System.Action<Collision> OnCollisionExit;
        public System.Action<Collision> OnCollisionStay;
    }

    [System.Serializable]
    public class TriggerCallback : UnityEvent<Collider>
    {
    }
    [System.Serializable]
    public class CollisionCallback : UnityEvent<Collision>
    {
    }
    [System.Serializable]
    public class UnityPhysicsCallbacks
    {
        public TriggerCallback OnTriggerEnter;
        public TriggerCallback OnTriggerExit;
        public TriggerCallback OnTriggerStay;
        public CollisionCallback OnCollisionEnter;
        public CollisionCallback OnCollisionExit;
        public CollisionCallback OnCollisionStay;
    }
    public class PhysicsEvents : MonoBehaviour
    {
        public UnityPhysicsCallbacks UnityEvents = new UnityPhysicsCallbacks();
        public PhysicsCallbacks Callbacks = new PhysicsCallbacks();
        public bool EnableDebug = false;

        void DebugMessage(string message)
        {
            if (EnableDebug)
            {
                Debug.Log(message);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Callbacks.OnTriggerEnter?.Invoke(other);
            UnityEvents.OnTriggerEnter?.Invoke(other);
            DebugMessage(gameObject.name + " Trigger enter with " + other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            Callbacks.OnTriggerStay?.Invoke(other);
            UnityEvents.OnTriggerStay?.Invoke(other);
            DebugMessage(gameObject.name + " Trigger stay with " + other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            Callbacks.OnTriggerExit?.Invoke(other);
            UnityEvents.OnTriggerExit?.Invoke(other);
            DebugMessage(gameObject.name + " Trigger exit with " + other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Callbacks.OnCollisionEnter?.Invoke(collision);
            UnityEvents.OnCollisionEnter?.Invoke(collision);
            DebugMessage(gameObject.name + " Collision enter with " + collision.collider.gameObject);
        }

        private void OnCollisionStay(Collision collision)
        {
            Callbacks.OnCollisionStay?.Invoke(collision);
            UnityEvents.OnCollisionStay?.Invoke(collision);
            DebugMessage(gameObject.name + " Collision stay with " + collision.collider.gameObject);

        }

        private void OnCollisionExit(Collision collision)
        {
            Callbacks.OnCollisionExit?.Invoke(collision);
            UnityEvents.OnCollisionExit?.Invoke(collision);
            DebugMessage(gameObject.name + " Collision exit with " + collision.collider.gameObject);

        }
    }
}