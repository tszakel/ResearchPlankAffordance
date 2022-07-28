using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RotateWithUser : MonoBehaviour
{
    //reference of head and reference of enviroment and and the whole camera rig
    public GameObject headReference, enviromentReference, rigReference, plank;
    private Vector3 headV, environmentV, plankStartingV;
    private Quaternion headQ, environmentQ;
    private bool updateStopper;

    private float plankEnd;

    // Start is called before the first frame update
    void Start()
    {
        environmentV = enviromentReference.transform.position;
        environmentQ = enviromentReference.transform.rotation;
        plankEnd = GetComponent<Renderer>().bounds.max.z;
        plankStartingV = new Vector3(0, plank.transform.position.y, plankEnd);
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
        }
    }
}
