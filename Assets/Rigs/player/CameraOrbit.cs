using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour {
    public PlayerController moveScript;
    
    private Camera cam;

    private float yaw = 0;
    private float pitch = 0;

    public float cameraSensitivityX = 10;
    public float cameraSensitivityY = 10;

    public float shakeIntensity = 0;

    private void Start()
    {
        
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        PlayerOrbitCamera();

        if (moveScript != null) transform.position = moveScript.transform.position;

        // if aiming, set camera rotation to look at target
        rotateCamToLookAtTarget();

        // zoom in camera
        zoomCamera();

        shakeCamera();

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Application.Quit();
        }
    }

    public void Shake(float intensity = 1, float timeMult = 1)
    {
        shakeIntensity = intensity;
    }

    private void shakeCamera()
    {
        if (shakeIntensity < 0) shakeIntensity = 0;
        if (shakeIntensity > 0) shakeIntensity -= Time.deltaTime;
        else return;

        // pick a small random rotation
        Quaternion targetRot = AnimMath.Lerp(Random.rotation, Quaternion.identity, .999f);

        //cam.transform.localRotation *= targetRot;
        cam.transform.localRotation = AnimMath.Lerp(cam.transform.localRotation, cam.transform.localRotation * targetRot, shakeIntensity * shakeIntensity);
    }

    private void zoomCamera()
    {
        float dis = 10;

        

        cam.transform.localPosition = AnimMath.Slide(cam.transform.localPosition, new Vector3(0, 0, -dis), .001f);

    }

    

    private void rotateCamToLookAtTarget()
    {

        
            // if not targeting, reset rotation
            cam.transform.localRotation = AnimMath.Slide(cam.transform.localRotation, Quaternion.identity, .01f); // no rotation
        
    }

    private void PlayerOrbitCamera()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        yaw += mx * cameraSensitivityX;
        pitch += my * cameraSensitivityY;

        
        pitch = Mathf.Clamp(pitch, 15, 89);
        

        transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(pitch, yaw, 0), .01f);
    }
}