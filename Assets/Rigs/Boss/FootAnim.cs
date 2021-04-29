using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootAnim : MonoBehaviour
{

    public Transform stepPosition;
    private DogController dogController;

    private Vector3 prevPlantedPosition;
    private Quaternion prevPlantedRotation = Quaternion.identity;

    private Vector3 plantedPosition;
    private Quaternion plantedRotation = Quaternion.identity;

    public float threshold = 2;
    private float halfThreshold;

    public Transform center;


    private float timeLength = .25f;
    private float timeCurrent = 0;

    public bool isAnimating
    {
        get
        {
            return (timeCurrent < timeLength);
        }
    }

    public AnimationCurve verticalMovement;
    public bool footHasMoved = false;

    // Start is called before the first frame update
    void Start()
    {
        dogController = GetComponentInParent<DogController>();
        halfThreshold = threshold / 2;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(isAnimating) // anim is playing
        {
            timeCurrent += Time.deltaTime;

            float p = timeCurrent / timeLength;

            Vector3 finalPos = AnimMath.Lerp(prevPlantedPosition, plantedPosition, p);

            finalPos.y += verticalMovement.Evaluate(p) * 2;

            transform.position = finalPos;
            transform.rotation = AnimMath.Lerp(prevPlantedRotation, plantedRotation, p);

        } else // anim is not playing
        {

            transform.position = plantedPosition;
            transform.rotation = plantedRotation;

        }
    }

    public bool TryToStep(bool overrideTrue = false)
    {
        if (!overrideTrue)
        {
            // if animating, don't step
            if (isAnimating) return false;

            Vector3 vBetween = transform.position - stepPosition.position;

            float vBetweenXAxis = transform.localPosition.x - center.localPosition.x;

            //if (dogController.isTurning)
            //{
            //if (vBetween.sqrMagnitude < halfThreshold * halfThreshold) return false;
            //}     

            if (dogController.isTurning)
            {
                threshold = .5f; 
                if (Mathf.Abs(vBetweenXAxis) < threshold && vBetween.sqrMagnitude < threshold * threshold) return false;
            }
            else
            {
                threshold = 2;
                if (vBetween.sqrMagnitude < threshold * threshold) return false;  
            } 
        }


        Ray ray = new Ray(stepPosition.position + Vector3.up, Vector3.down);

        //Debug.DrawRay(ray.origin, ray.direction * 3);

        if (Physics.Raycast(ray, out RaycastHit hit, 3))
        {
            //setup start of anim
            prevPlantedPosition = transform.position;
            prevPlantedRotation = transform.rotation;

            //setup end of anim
            plantedPosition = hit.point;
            plantedRotation = Quaternion.FromToRotation(transform.up, hit.normal);

            // begin anim
            timeCurrent = 0;
            footHasMoved = true;
            return true;    
        }
        return false;
    }
}
