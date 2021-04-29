using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script animates the foot / legs by
/// changing the local pisition of this object (IK target).
/// </summary>

public class FootAnimator : MonoBehaviour
{
    // localspace starting pos of the object
    private Vector3 startingPos;

    //an offset value used to control the timing of the sin wave for the walk anim
    public float stepOffset = 0;

    private Quaternion startRot;

    GoonController goon;

    private Vector3 targetPos;
    private Quaternion targetRot;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.localPosition;
        startRot = transform.localRotation;

        goon = GetComponentInParent<GoonController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (goon.state)
        {
            case GoonController.States.Idle:
                AnimateIdle();
                break;
            case GoonController.States.Walk:
                AnimateWalk();
                break;
        }

        //transform.position = AnimMath.Slide(transform.position, targetPos, .01f);
        //transform.rotation = AnimMath.Slide(transform.rotation, targetRot, .01f);
    }

    void AnimateWalk()
    {
        Vector3 finalPos = startingPos;

        float time = (Time.time + stepOffset) * goon.stepSpeed;

        //lateral movement (z + x)
        float frontToBack = Mathf.Sin(time);
        finalPos += goon.moveDir * frontToBack * goon.walkScale.z;

        //vertical movement (y)
        finalPos.y += Mathf.Cos(time) * goon.walkScale.y;

        //finalPos.x *= goon.walkScale.x;        

        bool isOnGround = (finalPos.y < startingPos.y);

        if (isOnGround)
        {
            finalPos.y = startingPos.y;
        }

        float p = 1 - Mathf.Abs(frontToBack);

        float anklePitch = isOnGround ? 0 : p * -10;

        transform.localPosition = finalPos;
        transform.localRotation = startRot * Quaternion.Euler(0, 0, anklePitch);

        //targetPos = transform.TransformPoint(finalPos);
        //targetRot = transform.parent.rotation * (startRot * Quaternion.Euler(0, 0, anklePitch));
    }

    void AnimateIdle()
    {
        transform.localPosition = startingPos;
        transform.localRotation = startRot;

        //targetPos = transform.TransformPoint(startingPos);
        //targetRot = transform.parent.rotation * startRot;
        FindGround();
    }

    void FindGround()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, .5f, 0), Vector3.down * 2);

        Debug.DrawRay(ray.origin, ray.direction);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        } else
        {

        }
    }

}
