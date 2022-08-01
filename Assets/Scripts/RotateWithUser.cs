using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RotateWithUser : MonoBehaviour
{
    //reference of head and reference of enviroment and and the whole camera rig
    public GameObject headReference, enviromentReference, rigReference, plank, calibrateScenePrompt, VRCamera, City;
    private Vector3 headV, environmentV, plankStartingV;
    private Quaternion headQ, environmentQ;
    private bool updateStopper, updateStopperGrip, trialStarted;

    private float plankEnd;

    public static Vector3 headPos;

    float x1,x2,y1,y2,z1,z2;
    // Start is called before the first frame update
    void Start()
    {
        VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        VRCamera.GetComponent<Camera>().backgroundColor = Color.black;
        City.SetActive(false);
        plank.SetActive(false);
        calibrateScenePrompt.SetActive(true);

        environmentV = enviromentReference.transform.position;
        environmentQ = enviromentReference.transform.rotation;
        plankEnd = plank.GetComponent<Renderer>().bounds.max.z;
        //Debug.Log("Starting head position: " + headV);
        plankStartingV = new Vector3(0, plank.transform.position.y, plankEnd);

        x1 = headReference.transform.position.x;
        y1 = headReference.transform.position.y;
        z1 = headReference.transform.position.z;

        x2 = headReference.transform.position.x;
        y2 = headReference.transform.position.y;
        z2 = headReference.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //at the start of every trial, take the transform value of the camera/head and get the rotation and the position
        //remove the y value from the positon (set it to 0 for the game object)
        /*Only care about the left and right of rotation (ie y axis) so set x and z of quat to 0
        rotation the environment's y axis to the y value of the camera quat (add the -180 default with it)
        set the environments position to the new camera/head 2d (x/z) positon
        // Take the starting point of the whole experiment, and put the participant back to that point every time
        // The teleportation issue (solve it later)
        */
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (updateStopper == false)
            {
                headV = headReference.transform.position;
                headV.y = 0;
                headQ = headReference.transform.rotation;
                headQ.x = 0;
                headQ.z = 0;
                headQ *= environmentQ;
                enviromentReference.transform.rotation = headQ;
                headV -= enviromentReference.transform.forward * (enviromentReference.transform.position.z);
                headV.y += enviromentReference.transform.position.y;
                enviromentReference.transform.position = headV;
                updateStopper = true;
            }
            headPos = headV;
            //Debug.Log("Starting head position: " + headV);
        }

        if (SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (updateStopperGrip == false)
            {
                VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
                City.SetActive(true);
                plank.SetActive(true);
                calibrateScenePrompt.SetActive(false);
                updateStopperGrip = false;
                trialStarted = true;
            }
        }

        if (trialStarted)
        {
            //headV += headReference.transform.position;
            x1 = headReference.transform.position.x;
            y1 = headReference.transform.position.y;
            z1 = headReference.transform.position.z;
            headV.x += x1-x2;
            headV.y += y1-y2;
            headV.z += z1-z2;
            /* headV.y = 0;
            headV -= enviromentReference.transform.forward * (enviromentReference.transform.position.z);
            headV.y += enviromentReference.transform.position.y; */
            headPos = headV;
            //Debug.Log(headV);
            x2 = headReference.transform.position.x;
            y2 = headReference.transform.position.y;
            z2 = headReference.transform.position.z;
        }

    }
}
