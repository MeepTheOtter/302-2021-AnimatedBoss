using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tailWag : MonoBehaviour
{

    private Vector3 startPos;
    private Quaternion startRot;

    private DogController dogController;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
        dogController = GetComponentInParent<DogController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (dogController.state == DogController.States.Idle)
        {



            Vector3 finalPos = startPos;

            float time = (Time.time - .5f) * 3f;

            //lateral movement (z + x)
            float frontToBack = Mathf.Sin(time);
            finalPos.x += frontToBack;

            //vertical movement (y)
            finalPos.y += Mathf.Cos(time - .5f) * 1.5f;



            float p = 1 - Mathf.Abs(frontToBack);



            transform.localPosition = AnimMath.Slide(transform.localPosition, finalPos, .01f);
            //transform.localRotation = startRot * Quaternion.Euler(0, 0, anklePitch);
        }
        else transform.localPosition = AnimMath.Slide(transform.localPosition, startPos, .001f);


    }
}
