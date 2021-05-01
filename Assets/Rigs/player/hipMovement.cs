using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hipMovement : MonoBehaviour
{

    private PlayerController player;
    private Quaternion startPos;
    private bool goLeft = false;
    public float speed = .3f;
    public float finalOffset = 2;
    public Quaternion deathPos;
    public float switchTimer = 0;
    public float mult = 2;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        startPos = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.state == PlayerController.States.Idle)
        {
            switchTimer -= Time.deltaTime;
            if(switchTimer <= 0)
            {
                switchTimer = 1;
                goLeft = !goLeft;
                transform.localRotation = startPos;
            }

            if (goLeft == true)
            {
                Quaternion targetRot = Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z - mult * Time.deltaTime);
                transform.localRotation = targetRot;
            }
            else
            {
                Quaternion targetRot = Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z + mult * Time.deltaTime);
                transform.localRotation = targetRot;
            }
        } 
        else if (player.state == PlayerController.States.Death)
        {
            transform.localRotation = Quaternion.Euler(90, 0, 0);
        }

        else
        {
            transform.localRotation = startPos;
        }
    }
}
