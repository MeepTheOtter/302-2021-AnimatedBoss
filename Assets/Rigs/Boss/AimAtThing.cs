using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtThing : MonoBehaviour
{

    private Vector3 startingPos;

    public jawAnim jawIK;
    private Vector3 jawIKOffset;

    public PlayerController player;

    public Transform headBone;

    //public float offset = 1;


    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.localPosition;
        jawIKOffset = transform.localPosition - jawIK.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //jawIK.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + offset , transform.localPosition.z);
        transform.position = AnimMath.Slide(transform.position, (player.transform.position + new Vector3(0,1,0)), .01f);

        Vector3 angle = Quaternion.ToEulerAngles(transform.rotation);
        if(angle.z > 70)
        {
            transform.rotation = Quaternion.Euler(angle.x, angle.y, 70);
        }
        if (angle.z < -20)
        {
            transform.rotation = Quaternion.Euler(angle.x, angle.y, -20);
        }
        if (angle.x > 10)
        {
            transform.rotation = Quaternion.Euler(10, angle.y, angle.z);
        }
        if (angle.x < -10)
        {
            transform.rotation = Quaternion.Euler(-10, angle.y, angle.z);
        }
        if (angle.y > 75)
        {
            transform.rotation = Quaternion.Euler(angle.x, 75, angle.z);
        }
        if (angle.y < -75)
        {
            transform.rotation = Quaternion.Euler(angle.x, -75, angle.z);
        }

        jawIK.transform.position = new Vector3(transform.position.x, transform.position.y + jawIK.offset, transform.position.z);
    }
}
