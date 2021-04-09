using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickyFoot : MonoBehaviour
{

    public Transform stepPosition;
    public AnimationCurve verticalMovement;

    private Vector3 prevPlantedPosition;
    private Quaternion prevPlantedRotation;

    private Vector3 plantedPosition;
    private Quaternion plantedRotation;

    private float timeLength = .25f;
    private float timeCurrent = 0;


    void Start()
    {
        
    }

    
    void Update()
    {
        if(CheckIfCanStep())
        {
            DoRaycast();
        }

        if(timeCurrent < timeLength) // animation is playing
        {
            timeCurrent += Time.deltaTime; //move playhead forward
            float p = timeCurrent / timeLength;


            Vector3 finalPosition = AnimMath.Lerp(prevPlantedPosition, plantedPosition, p);

            finalPosition.y += verticalMovement.Evaluate(p);

            transform.position = finalPosition;

            transform.rotation = AnimMath.Lerp(prevPlantedRotation, plantedRotation, p);



        } else //anim is not playing
        {
            transform.position = plantedPosition;
            transform.rotation = plantedRotation;
        }        
    }

    bool CheckIfCanStep()
    {
        Vector3 vBetween = transform.position - stepPosition.position;
        float threshold = 5;

        return (vBetween.sqrMagnitude > threshold * threshold);
    }

    void DoRaycast()
    {
        Ray ray = new Ray(stepPosition.position + Vector3.up, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction * 3);

        if(Physics.Raycast(ray, out RaycastHit hit, 3)) 
        {

            prevPlantedPosition = transform.position;
            prevPlantedRotation = transform.rotation;

            plantedPosition = hit.point;
            plantedRotation = Quaternion.FromToRotation(transform.up, hit.normal);

            timeCurrent = 0;
        }
    }
}
