using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianMovement : MonoBehaviour
{

    public Animator animMachine;
    

    void Start()
    {
        animMachine = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Input.GetAxisRaw("Vertical");        

        transform.position += transform.forward * speed * Time.deltaTime * 3;

        speed = Mathf.Abs(speed);
        animMachine.SetFloat("current speed", speed);
    }
}
