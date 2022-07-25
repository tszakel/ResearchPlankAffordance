using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Valve.VR;
using TMPro;



public class DetectFall : MonoBehaviour
{
    private float plankRightBound;
    private float plankLeftBound;
    private float plankEnd;
    private float plankStart;

    
    private GameObject rightFoot;
    private GameObject leftFoot;
    private float rightFootCenter;
    private float leftFootCenter;
    
    private GameObject player;
    private float playerCenter;
    
    public static bool hasFallen;
    public static bool successfulTrial;

    private float bodyWiggleRoom = 0.381f;

    private float HMDTracker;
    private float lateralDifference = 0.0f;

    public GameObject VRCamera,City,Instructions,respawnLeftTrigger,respawnRightTrigger; 
    private TextMeshProUGUI alterInstructions;

    Vector3 leftDetect;
    Vector3 rightDetect;

    public Renderer rend;
    

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        
        HMDTracker = VRCamera.transform.position.z;
        plankStart = GetComponent<Renderer>().bounds.min.x + 5;
        
        Debug.Log("start" + plankStart);


        
        //respawnLeftTrigger.transform.position.x 
        //leftDetect = HMDTracker - PlankChange.plankExtent - bodyWiggleRoom; // x value


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
        leftDetect = respawnLeftTrigger.transform.position;
        leftDetect.x = HMDTracker - PlankChange.plankExtent - bodyWiggleRoom;
        respawnLeftTrigger.transform.position = leftDetect;

        rightDetect = respawnRightTrigger.transform.position;
        rightDetect.x = HMDTracker + PlankChange.plankExtent + bodyWiggleRoom;
        respawnRightTrigger.transform.position = rightDetect;
        
        lateralDifference = Mathf.Abs((HMDTracker - VRCamera.transform.position.x) + bodyWiggleRoom);
        Debug.Log(lateralDifference);
        //Debug.Log(lateralDifference);

        Fallen();
        //monitorSuccessfulTrial(HMDTracker);

       
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

    void monitorSuccessfulTrial(float userCenter) {
        if(userCenter <= plankEnd){
            VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            VRCamera.GetComponent<Camera>().backgroundColor = Color.black;
            City.SetActive(false);
            alterInstructions.text = "You have successfully crossed.\n Please wait to be guided back and press the trigger when you're ready to begin the next trial.";
            Instructions.SetActive(true);
            rend.enabled = false;

            successfulTrial = true;
        }
    }
    
    void Fallen() {
        if ((lateralDifference > PlankChange2.plankExtent + bodyWiggleRoom) &&  (VRCamera.transform.position.z > plankStart)){
            // Debug.Log("lateralDif: " + lateralDifference + " extent: " + PlankChange.plankExtent);

            VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            VRCamera.GetComponent<Camera>().backgroundColor = Color.black;
            City.SetActive(false);
            Instructions.SetActive(true);
            rend.enabled = false;

            hasFallen = true;
        }
    }
    
}
