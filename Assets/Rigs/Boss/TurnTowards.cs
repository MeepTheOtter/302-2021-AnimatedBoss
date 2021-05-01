using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowards : MonoBehaviour
{

    public PlayerController player;
    private DogController dog;
    public Transform forwardVector;

    // Start is called before the first frame update
    void Start()
    {
        dog = GetComponentInParent<DogController>();
    }

    // Update is called once per frame
    void Update()
    {
        print(Vector3.Angle(forwardVector.position - transform.position, player.transform.position - transform.position));

        float angle = 5;
        float wideTurnAngle = 65;
        if (Vector3.Angle(forwardVector.position - transform.position, player.transform.position - transform.position) < angle)
        {
            dog.turnSpeed = 0;
        } else if (Vector3.Angle(forwardVector.position - transform.position, player.transform.position - transform.position) > wideTurnAngle)
        {
            if (Vector3.Angle(transform.right, player.transform.position - transform.position) < Vector3.Angle(-transform.right, player.transform.position - transform.position))
            {
                dog.turnSpeed = 50;
            }
            else
            {
                dog.turnSpeed = -50;
            }
        } else
        {
            if(Vector3.Angle(transform.right, player.transform.position - transform.position) < Vector3.Angle(-transform.right, player.transform.position - transform.position))
            {
                dog.turnSpeed = 15;
            } else
            {
                dog.turnSpeed = -15;
            }
        }
    }
}
