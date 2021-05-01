using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    DogController dog;

    // Start is called before the first frame update
    void Start()
    {
        dog = GetComponentInParent<DogController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        HealthSystem health = other.GetComponent<HealthSystem>();
        if (player != null && health != null)
        {
            dog.playerIsInZone = true;
            //health.takeDamage(20, 5);
            //print("owwwww");
        }
    }

    
}
