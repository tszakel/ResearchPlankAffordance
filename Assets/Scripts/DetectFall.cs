using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Valve.VR;


public class DetectFall : MonoBehaviour
{
    private float plankRightBound;
    private float plankLeftBound;
    private float plankEnd;
    
    private GameObject rightFoot;
    private GameObject leftFoot;
    private float rightFootCenter;
    private float leftFootCenter;
    
    private GameObject player;
    private float playerCenter;
    
    public static bool hasFallen;
    public static bool successfulTrial;

    //private float bodyWiggleRoom = 0.8f;

    public float HMDTracker;
    private float lateralDifference = 0.0f;

    public GameObject VRCamera,City; 
    Vector3 startPos;

    public Renderer rend;
    

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        
        // startPos = VRCamera.transform.position;
        // Debug.Log("Start Position: " + startPos);
        HMDTracker = VRCamera.transform.position.z;
        //Debug.Log(HMDTracker);
        /*plankLeftBound = GetComponent<Renderer>().bounds.min.z;
        plankRightBound = GetComponent<Renderer>().bounds.max.z;
        plankEnd = GetComponent<Renderer>().bounds.min.x;
        
        rightFoot = GameObject.FindWithTag("rightFoot");
        leftFoot = GameObject.FindWithTag("leftFoot");
        
        rightFootCenter = rightFoot.GetComponent<Renderer>().bounds.center.z;
        leftFootCenter = leftFoot.GetComponent<Renderer>().bounds.center.z;
        
        player = GameObject.FindWithTag("Player");
        playerCenter = player.GetComponent<Renderer>().bounds.center.x;*/
        
        //Debug.Log("right foot position: " + rightFootCenter + " left foot position: " + leftFootCenter);



        /*Debug.Log("plankLeftBound: " + plankLeftBound + " " + "plankRightBound: " + plankRightBound + " " +
                  "plankEnd: " + plankEnd + " " + "rightFootCenter: " + rightFootCenter + " " +
                  "leftFootCenter: " + leftFootCenter + " " + "playerCenter: " + playerCenter);*/

        //Debug.Log("X: " + plankSize.x + " " + "Y: " + plankSize.y + " " + "Z: " + plankSize.z);
    }

    // Update is called once per frame
    void Update()
    {
        
        lateralDifference = Mathf.Abs((HMDTracker - VRCamera.transform.position.x));
        if (lateralDifference > PlankChange.plankExtent)
        {
            Fallen();
            // Debug.Log("lateralDif: " + lateralDifference + " extent: " + PlankChange.plankExtent);
            // Debug.Log("you have fallen " + hasFallen);

        }

        // if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any)){
        //     Debug.Log("trig");
        //     VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        //     City.SetActive(true);
        //     rend.enabled = true;
        // }

        // if (SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any)){
        //     Debug.Log("grip");

        //     VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        //     VRCamera.GetComponent<Camera>().backgroundColor = Color.black;
        //     City.SetActive(false);
        //     rend.enabled = false;
        // }
        // //Debug.Log(lateralDifference);
        /*plankLeftBound = GetComponent<Renderer>().bounds.min.z;
        plankRightBound = GetComponent<Renderer>().bounds.max.z;
        plankEnd = GetComponent<Renderer>().bounds.min.x;

        rightFootCenter = rightFoot.GetComponent<Renderer>().bounds.center.z;
        leftFootCenter = leftFoot.GetComponent<Renderer>().bounds.center.z;
        
        playerCenter = player.GetComponent<Renderer>().bounds.center.x;*/

        //MonitorFeet(rightFootCenter,leftFootCenter);
        //checkWithinBounds();
        //monitorSuccessfulTrial(playerCenter);

    }

    // private void MonitorFeet(float rightFoot, float leftFoot)
    // {
    //     if((checkWithinBounds(rightFoot) == false && checkWithinBounds(leftFoot)) == false){
    //         Fallen();
    //     }
    // }
    
    // bool checkWithinBounds(float foot) {
    //     if (foot <= plankRightBound && foot >= plankLeftBound) {
    //         Debug.Log("you're within bounds");
    //         return true;
    //     }
    //     return false;
    // }

    void monitorSuccessfulTrial(float userCenter) {
        if(userCenter <= plankEnd){
            VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            VRCamera.GetComponent<Camera>().backgroundColor = Color.black;
            City.SetActive(false);
            this.gameObject.SetActive(false);

            successfulTrial = true;
        }
    }
    
    void Fallen() {
        VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        VRCamera.GetComponent<Camera>().backgroundColor = Color.black;
        City.SetActive(false);
        rend.enabled = false;

        hasFallen = true;
    }
    
}
