using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{

    public enum States
    {
        Idle,
        Walk,
        Attack,
        Death,
    }

    public Transform groundRing;
    CharacterController pawn; 
    public bool isSideStepping = false;
    public bool isTurning = false;
    private bool moveLeftFoot = true;

    public FootAnim frontLeft;
    public FootAnim frontRight;
    public FootAnim backLeft;
    public FootAnim backRight;

    public float turnSpeed = 2;

    //changing this quaternion will rotate the dog & the feet correctly
    public Vector3 turnRotation = new Vector3(0, 0, 0);

    public float speed = 5;

    public States state;

    private void Start()
    {
        state = States.Idle;
        pawn = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (state != States.Death)
        {
            Move();

            if (moveLeftFoot && !frontRight.isAnimating)
            {
                if (frontLeft.TryToStep())
                {
                    backRight.TryToStep(true);
                    moveLeftFoot = !moveLeftFoot;
                }
            }
            else if (!frontLeft.isAnimating)
            {
                if (frontRight.TryToStep())
                {
                    backLeft.TryToStep(true);
                    moveLeftFoot = !moveLeftFoot;
                }
            }
            Vector3 prevRot = turnRotation;
            turnRotation.y = turnSpeed * Time.deltaTime;
            //print(turnRotation.y);
            if (turnRotation != prevRot) isTurning = true;
            //Quaternion.RotateTowards(transform.rotation, turnRotation, 15);
            //AnimMath.Lerp(transform.rotation, turnRotation, .001f);
            transform.Rotate(0, 0, turnRotation.y);
        }

        
    }

    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        if (v != 0 || h != 0) state = States.Walk;
        else state = States.Idle;
        if (h != 0) isSideStepping = true;
        else isSideStepping = false;
        //print(isTurning);

        Vector3 velocity = transform.up * -v + transform.right * h * .2f;
        //print(velocity);        

        pawn.SimpleMove(velocity * speed);

        Vector3 localVelocity = groundRing.InverseTransformDirection(velocity);

        Vector3 adjustedVelocity = new Vector3(localVelocity.x * .2f, localVelocity.y , localVelocity.z );

        groundRing.localPosition = AnimMath.Slide(groundRing.localPosition, adjustedVelocity * 1.5f, .0001f);
    }
}
