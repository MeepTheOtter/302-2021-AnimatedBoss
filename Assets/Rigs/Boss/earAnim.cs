using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earAnim : MonoBehaviour
{

    private Vector3 startingPos;

    public float wiggleTimer = 0;
    public AnimationCurve wiggleCurve;

    private float timeLength = .25f;
    private float timeCurrent = 0;

    public bool isAnimating
    {
        get
        {
            return (timeCurrent < timeLength);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (wiggleTimer <= 0)
        {
            WiggleEars();
        }
        if (isAnimating) // anim is playing
        {
            timeCurrent += Time.deltaTime;

            float p = timeCurrent / timeLength;

            Vector3 newPos = new Vector3(startingPos.x + Random.Range(-.5f, .5f), startingPos.y, startingPos.z + Random.Range(-.5f, .5f));

            Vector3 finalPos = AnimMath.Lerp(startingPos, newPos, wiggleCurve.Evaluate(p));
            

            transform.localPosition = finalPos;
            

        } else
        {
            transform.localPosition = startingPos;
        }
        
        if (wiggleTimer > 0) wiggleTimer -= Time.deltaTime;
    }

    void resetTimer()
    {
        wiggleTimer = Random.Range(1, 50);
    }

    void WiggleEars()
    {
        resetTimer();
        timeCurrent = 0;
    }

}
