using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public Transform groundRing;
    CharacterController pawn; 
    public bool isTurning = false;
    private bool moveLeftFoot = true;

    public FootAnim frontLeft;
    public FootAnim frontRight;
    public FootAnim backLeft;
    public FootAnim backRight;

    public float speed = 5;

    private void Start()
    {
        pawn = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();

        if(moveLeftFoot && !frontRight.isAnimating)
        {
            if(frontLeft.TryToStep())
            {
                backRight.TryToStep(true);
                moveLeftFoot = !moveLeftFoot;
            }
        } else if (!frontLeft.isAnimating)
        {
            if(frontRight.TryToStep())
            {
                backLeft.TryToStep(true);
                moveLeftFoot = !moveLeftFoot;
            }
        }
        

        

        
    }

    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0) isTurning = true;
        else isTurning = false;
        //print(isTurning);

        Vector3 velocity = transform.up * -v + transform.right * h * .2f;
        //print(velocity);        

        pawn.SimpleMove(velocity * speed);

        Vector3 localVelocity = groundRing.InverseTransformDirection(velocity);

        Vector3 adjustedVelocity = new Vector3(localVelocity.x * .2f, localVelocity.y , localVelocity.z );

        groundRing.localPosition = AnimMath.Slide(groundRing.localPosition, adjustedVelocity * 1.5f, .0001f);
    }
}
