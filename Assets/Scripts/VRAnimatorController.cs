using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnimatorController : MonoBehaviour
{
    public float speedThreshold = 0.1f;
    [Range (0,1)]
    public float smoothing;
    private Animator animator;
    private Vector3 previusPos;
    private VRRig vrRig;

    void Start()
    {
        animator = GetComponent<Animator>();
        vrRig = GetComponent<VRRig>();
        previusPos = vrRig.head.vrTarget.position;
    }

    void Update()
    {
        //Compute the speed
        Vector3 headsetSpeed = (vrRig.head.vrTarget.position - previusPos) / Time.deltaTime;
        headsetSpeed.y = 0;
        //Local speed
        Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);
        previusPos = vrRig.head.vrTarget.position;

        //Set Animator Values
        float previusDirectionX = animator.GetFloat("DirectionX");
        float previusDirectionY = animator.GetFloat("DirectionY");


        animator.SetBool("isMoving", headsetLocalSpeed.magnitude > speedThreshold);
        animator.SetFloat("DirectionX", Mathf.Lerp( previusDirectionX, Mathf.Clamp(headsetLocalSpeed.x, -1, 1), smoothing));
        animator.SetFloat("DirectionY", Mathf.Lerp(previusDirectionY, Mathf.Clamp(headsetLocalSpeed.z, -1, 1), smoothing));
    }
}
