using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowards : MonoBehaviour
{

    public PlayerController player;
    private DogController dog;

    // Start is called before the first frame update
    void Start()
    {
        dog = GetComponentInParent<DogController>();
    }

    // Update is called once per frame
    void Update()
    {

        float angle = 10;
        if (Vector3.Angle(transform.forward, player.transform.position - transform.position) < angle)
        {
            dog.turnSpeed = 0;
        } else
        {
            if(Vector3.Angle(transform.right, player.transform.position - transform.position) < Vector3.Angle(-transform.right, player.transform.position - transform.position))
            {
                dog.turnSpeed = 10;
            } else
            {
                dog.turnSpeed = -10;
            }
        }
    }
}
