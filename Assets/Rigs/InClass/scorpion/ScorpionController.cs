using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionController : MonoBehaviour
{

    private CharacterController pawn;

    public Transform groundRing;

    public List<stickyFoot> feet = new List<stickyFoot>();

    // Start is called before the first frame update
    void Start()
    {
        pawn = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        int feetStepping = 0;
        int feetMoved = 0;
        foreach (stickyFoot foot in feet)
        {
            if (foot.isAnimating) feetStepping++;
            if (foot.footHasMoved) feetMoved++;
        } 

        if(feetMoved >= 8)
        {
            foreach(stickyFoot foot in feet)
            {
                foot.footHasMoved = false;
            }
        }

        foreach (stickyFoot foot in feet)
        {
            if (feetStepping < 4)
            {
                if (foot.TryToStep()) feetStepping++;
            }
        }
    }

    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 velocity = transform.forward * v;
        //velocity.Normalize();
        pawn.SimpleMove(velocity * 5);
        //print(velocity);

        transform.Rotate(0, h * 90 * Time.deltaTime, 0);


        Vector3 localVelocity = groundRing.InverseTransformDirection(velocity);

        groundRing.localPosition = AnimMath.Slide(groundRing.localPosition, localVelocity * 3, .0001f);

        groundRing.localEulerAngles = new Vector3(0, h * 30, 0);
    }
}
