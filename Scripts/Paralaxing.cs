using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxing : MonoBehaviour
{
    public Transform[] backgrounds; //arrat list all the back for paralax
    private float[] parallaxScales; // prorportion of the cameras movement
    public float smoothing = 1f; // How smoot the parallax is going to be, Make sure 
    private Transform cam; // reference to the mian cameras transform
    private Vector3 previousCamPos; // the pos of the cam in the first frame
    
    private void Awake()
    {
        // set up camera ref
        cam = Camera.main.transform;
    }
    void Start()
    {
        previousCamPos = cam.position;

        //asigning coresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i ++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i ++)
        {
            //the parallax is the opposite of the camera movement 
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            //set a target x pos which is the current pos plus the parallax
            float backgroundPosX = backgrounds[i].position.x + parallax;

            //create a target pos whick is the vackground curren pos with its taget x pos
            Vector3 backgroundTargetPos = new Vector3(backgroundPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //fade between current pos and the target pos using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        //set the previousCamPos to the Cameras pos at the end of the frame
        previousCamPos = cam.position;
    }
}
