using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum States
    {
        Idle,
        Walk,
        Attack,
        Death,
    }

    private Camera cam;
    private CharacterController pawn;
    public DogController dog;

    public float walkSpeed = 5;
    private float walkSpeedReset = 5;
    private float runSpeed = 10;
    
    public float idleTimer = 0;

    
    bool isShiftHeld = false;

    private Vector3 inputDirection = new Vector3();

    private float verticalVelocity = 0;
    private float horizontalVelocity = 0;

    public float gravityMult = 10;
    public float jumpMult = 5;

    private float coyoteTime = 0;

    public States state;

    public bool isGrounded
    {
        get
        { // return true if pawn is on the ground OR "coyote-time" isn't zero
            return pawn.isGrounded || coyoteTime > 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        pawn = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state != States.Death)
        {
            if (coyoteTime > 0) coyoteTime -= Time.deltaTime;

            if (idleTimer > 0) idleTimer -= Time.deltaTime;
            if (idleTimer <= 0)
            {
                idleTimer = .6f;

            }

            movePlayer();
            if (isGrounded) wiggleLegs(); // idle + walk

            tryToAttack();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Application.Quit();
        }
    }

    

    private void wiggleLegs()
    {
        float degrees = 45;
        float speed = 10;

        Vector3 inputDirLocal = transform.InverseTransformDirection(inputDirection);
        Vector3 axis = Vector3.Cross(inputDirLocal, Vector3.up);

        // check the alignment of inputDirLocal against forward vector
        float alignment = Vector3.Dot(inputDirLocal, Vector3.forward);
        alignment = Mathf.Abs(alignment);

        degrees *= AnimMath.Lerp(.25f, 1, alignment); //decrease 'degrees' when strafing


        float wave = Mathf.Sin(Time.time * speed) * degrees;

        state = States.Idle;

        if (isShiftHeld)
        {
            //IMPLEMENT ARM SWING CODE
        }
    }

    

    

    private void movePlayer()
    {
        float h = Input.GetAxis("Horizontal"); // strafing?
        float v = Input.GetAxis("Vertical"); // forward / backward

        isShiftHeld = Input.GetKey(KeyCode.LeftShift);

        bool isJumpHeld = Input.GetButton("Jump");
        bool onJumpPress = Input.GetButtonDown("Jump");

        if (isShiftHeld) walkSpeed = runSpeed;
        else if (!isShiftHeld) walkSpeed = walkSpeedReset;

        bool isTryingToMove = (h != 0 || v != 0);
        if (isTryingToMove)
        {
            // turn to face the correct direction...
            float camYaw = cam.transform.eulerAngles.y;
            transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(0, camYaw, 0), .02f);
            state = States.Walk;
        }
        else state = States.Idle;

        

        inputDirection = transform.forward * v + transform.right * h;
        if (inputDirection.sqrMagnitude > 1) inputDirection.Normalize();

        //apply gravity
        verticalVelocity += gravityMult * Time.deltaTime;

        //adds lateral movement to vertical
        Vector3 moveDelta = inputDirection * walkSpeed + verticalVelocity * Vector3.down;

        //move pawn
        CollisionFlags flags = pawn.Move(moveDelta * Time.deltaTime);
        if (pawn.isGrounded)
        {
            verticalVelocity = 0; // on ground, zero out vertical velocity
            coyoteTime = .2f;
        }


        if (isGrounded)
        {
            if (isJumpHeld)
            {
                verticalVelocity = -jumpMult;
                coyoteTime = 0;
            }
        }
    }

    void tryToAttack()
    {
        bool wantsToAttack = Input.GetButtonDown("Fire1");

        if (!wantsToAttack) return;

        if (Vector3.Distance(transform.position, dog.transform.position) > 20) return;

        float angle = 80;
        if (Vector3.Angle(transform.forward, dog.transform.position - transform.position) > angle)
        {
            return;
        }

        HealthSystem health = dog.GetComponent<HealthSystem>();

        if (health.iframes > 0) return;

        health.takeDamage(20, 0);
    }
}
