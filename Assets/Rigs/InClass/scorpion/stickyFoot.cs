using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;

public class stickyFoot : MonoBehaviour
{
    //how far away to allow the foot to slide
    public static float moveThreshold = 1;

    public Transform stepPosition;
    public AnimationCurve verticalMovement;

    private Quaternion startingRotation;

    private Vector3 prevPlantedPosition;
    private Quaternion prevPlantedRotation;

    private Vector3 plantedPosition;
    private Quaternion plantedRotation;

    private float timeLength = .25f;
    private float timeCurrent = 0;

    public bool isAnimating
    {
        get
        {
            return (timeCurrent < timeLength);
        }
    }

    public bool footHasMoved = false;

    Transform kneePole;

    void Start()
    {
        kneePole = transform.GetChild(0);

        startingRotation = transform.localRotation;
    }

    
    void Update()
    {        

        if(isAnimating) // animation is playing
        {
            timeCurrent += Time.deltaTime; //move playhead forward
            float p = timeCurrent / timeLength;


            Vector3 finalPosition = AnimMath.Lerp(prevPlantedPosition, plantedPosition, p);

            finalPosition.y += verticalMovement.Evaluate(p);

            transform.position = finalPosition;

            transform.rotation = AnimMath.Lerp(prevPlantedRotation, plantedRotation, p);

            Vector3 vFromCenter = transform.position - transform.parent.position;

            vFromCenter.y = 0;
            vFromCenter.Normalize();
            vFromCenter *= 3;
            vFromCenter.y += 2.5f;
            vFromCenter += transform.position;

            kneePole.position = vFromCenter;

        } else //anim is not playing
        {
            transform.position = plantedPosition;
            transform.rotation = plantedRotation;
        }        
    }

    public bool TryToStep()
    {
        if (isAnimating) return false;

        if (footHasMoved) return false;

        Vector3 vBetween = transform.position - stepPosition.position;

        if (vBetween.sqrMagnitude < moveThreshold * moveThreshold) return false;


  
        Ray ray = new Ray(stepPosition.position + Vector3.up, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction * 3);

        if(Physics.Raycast(ray, out RaycastHit hit, 3)) 
        {

            prevPlantedPosition = transform.position;
            prevPlantedRotation = transform.rotation;

            transform.localRotation = startingRotation;

            plantedPosition = hit.point;
            plantedRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;


            timeCurrent = 0;

            footHasMoved = true;
            return true;
        }
        return false;
    }
}
