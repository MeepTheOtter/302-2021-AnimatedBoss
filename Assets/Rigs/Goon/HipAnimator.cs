using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipAnimator : MonoBehaviour
{
    public float rollAmount = 20;

    GoonController goon;
    Quaternion startRot;

    // Start is called before the first frame update
    void Start()
    {
        goon = GetComponentInParent<GoonController>();
        startRot = transform.localRotation;
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
    }

    void AnimateIdle()
    {
        transform.localRotation = startRot;
    }
    
    void AnimateWalk()
    {
        float time = Time.time * goon.stepSpeed;
        float roll = Mathf.Sin(time) * rollAmount;

        transform.localRotation = Quaternion.Euler(0, 0, roll);
    }
}
