using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
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

    public GameObject jawBone;
    private Quaternion jawStartRot;

    private float timeLengthDown = .25f;
    private float timeCurrentDown = .25f;

    public bool isAnimatingDown
    {
        get
        {
            return (timeCurrentDown < timeLengthDown);
        }
    }

    private float timeLengthUp = .25f;
    private float timeCurrentUp = .25f;
    private bool canAnimateUp = false;

    public bool isAnimatingUp
    {
        get
        {
            return (timeCurrentUp < timeLengthUp);
        }
    }

    public float idleTimer = 5;

    private NavMeshAgent agent;
    public Transform player;

    public PlayerController playerController;
    public HealthSystem playerHealth;
    public bool playerIsInZone = false;
    

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
    public float distAway = 20;

    public float dampen = .5f;

    //changing this quaternion will rotate the dog & the feet correctly
    public Vector3 turnRotation = new Vector3(0, 0, 0);

    public float speed = 5;

    public States state;

    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();

        agent.updateRotation = false;
        state = States.Idle;
        pawn = GetComponent<CharacterController>();
        jawStartRot = jawBone.transform.localRotation;
        playerController = player.GetComponent<PlayerController>();
        playerHealth = player.GetComponent<HealthSystem>();
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

            if (idleTimer > 0 && state == States.Idle) idleTimer -= Time.deltaTime;

            if (isAnimatingDown) timeCurrentDown += Time.deltaTime;
            if (isAnimatingUp) timeCurrentUp += Time.deltaTime;


            if (state == States.Idle && idleTimer <= 0)
            {
                if (!isAnimatingDown)
                {
                    timeCurrentDown = 0;
                    //print("startAnim");

                }
            }

            if (isAnimatingDown)
            {

                float p = timeCurrentDown / timeLengthDown;

                Quaternion targetRot = Quaternion.Euler(0, 0, -105);

                jawBone.transform.localRotation = AnimMath.Lerp(jawBone.transform.localRotation, targetRot, p);
                canAnimateUp = true;
            }

            if (canAnimateUp && !isAnimatingDown)
            {
                timeCurrentUp = 0;
                canAnimateUp = false;
                state = States.Attack;
                if(playerIsInZone) playerHealth.takeDamage(20, 0);
            }

            if (isAnimatingUp)
            {
                float p = timeCurrentUp / timeLengthUp;

                Quaternion targetRot = Quaternion.Euler(0, 0, -60);

                jawBone.transform.localRotation = AnimMath.Lerp(jawBone.transform.localRotation, targetRot, p);
                
            }

            if (!isAnimatingUp && !isAnimatingDown)
            {
                jawBone.transform.localRotation = AnimMath.Slide(jawBone.transform.localRotation, jawStartRot, .001f);
                if (state == States.Attack) state = States.Idle;
            }


            print(playerIsInZone);



            if (idleTimer < 0 || state != States.Idle) idleTimer = 5;
            
        }

        
    }

    private void Move()
    {
        float dis = Vector3.Distance(transform.position, player.transform.position);
        //print(dis);
        if (dis < distAway)
        {
            state = States.Idle;
            agent.SetDestination(transform.position);

        }
        else if (dis > distAway)
        {
            state = States.Walk;
            agent.SetDestination(player.position);

        }

        if (dis > distAway) playerIsInZone = false;


        //print(dampen);




        //float v = Input.GetAxisRaw("Vertical");
        //float h = Input.GetAxisRaw("Horizontal");
        //if (v != 0 || h != 0) state = States.Walk;
        //else state = States.Idle;
        //if (h != 0) isSideStepping = true;
        //else isSideStepping = false;
        //print(isTurning);
        if (dis < 5) dampen = .1f;
        Vector3 velocity = agent.velocity * dampen;
        //print(velocity);        

        //pawn.SimpleMove(velocity * speed);

        Vector3 localVelocity = groundRing.InverseTransformDirection(velocity);

        Vector3 adjustedVelocity = new Vector3(localVelocity.x * .2f, localVelocity.y , localVelocity.z );

        groundRing.localPosition = AnimMath.Slide(groundRing.localPosition, adjustedVelocity * 1.5f, .0001f);
    }
}
