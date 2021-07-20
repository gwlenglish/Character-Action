using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRootMotion
{
    void SetRootMotionActive(bool isActive);

}
public class RootMotionScript : MonoBehaviour, IRootMotion
{
    Transform controller;
    public bool apply;
    Animator animator;
    Vector3 lastpos;
    private void Awake()
    {
        controller = transform.parent.GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

   
    private void LateUpdate()
    {
        lastpos = controller.transform.position;
    }
    void OnAnimatorMove()
    {
        if (animator && apply)
        {
            Vector3 newPosition = transform.parent.position;
            newPosition.y += animator.deltaPosition.y;
            newPosition.x += animator.deltaPosition.x;
            newPosition.z += animator.deltaPosition.z;
            transform.parent.position = newPosition;
            //need to handle collisions and exiting.
        }
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator && apply == false)
        {
            animator.bodyPosition = Vector3.MoveTowards(lastpos, transform.parent.position, Time.deltaTime);
          //  animator.bodyPosition = transform.parent.position;
        }
    }

   

    public void SetRootMotionActive(bool isActive)
    {
        apply = isActive;
    }
}
