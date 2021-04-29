using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jawAnim : MonoBehaviour
{

    private Vector3 startPos;
    private Vector3 highPos;
    private Vector3 lowPos;

    public bool openJaw = false;

    //change this to move the jaw up and down
    // -.75 = closed
    // -1.3 = small open
    // -11 = bite
    public float offset = -.75f;

    public Transform jawSave;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = jawSave.position;
        lowPos = jawSave.localPosition;
        lowPos.x = jawSave.localPosition.x + 8;

        if (openJaw) jawSave.localPosition = AnimMath.Slide(jawSave.localPosition, lowPos, .001f);
        else
        {
            jawSave.localPosition = AnimMath.Slide(lowPos, new Vector3(jawSave.localPosition.x - 8, jawSave.localPosition.y, jawSave.localPosition.z), .001f);
        }
    }
}
