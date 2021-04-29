using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipAnim : MonoBehaviour
{
    private DogController dog;
    private Vector3 startPos;
    private bool goDown = false;
    public float speed = .3f;
    public float finalOffset = 2;
    public Vector3 deathPos;

    // Start is called before the first frame update
    void Start()
    {
        dog = GetComponentInParent<DogController>();
        startPos = transform.localPosition;
        deathPos = startPos - new Vector3(0, .3f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (dog.state == DogController.States.Idle)
        {
            if (transform.localPosition.y > startPos.y)
            {
                //transform.localPosition.y -= .01f * Time.deltaTime;
                goDown = true;
            }
            if (transform.localPosition.y < startPos.y - .3f)
            {
                //transform.localPosition.y -= .01f * Time.deltaTime;
                goDown = false;
            }
            if (goDown == true)
            {
                transform.localPosition -= new Vector3(0, speed * Time.deltaTime, 0);
            }
            else
            {
                transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
            }

        }
        else if (dog.state == DogController.States.Death)
        {
            transform.localPosition = deathPos;
        }
        else
        {
            transform.localPosition = AnimMath.Lerp(transform.localPosition, startPos, .01f);
        }
    }
}
