using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Valve.VR;
using TMPro;



public class DetectFall : MonoBehaviour
{
    private float plankEnd, plankStart;
    
    public static bool hasFallen, successfulTrial;

    private float bodyWiggleRoom = 0.381f;

    private float HMDTracker;
    private float lateralDifference = 0.0f;

    public GameObject VRCamera,City,Instructions,actionPrompt,respawnLeftTrigger,respawnRightTrigger; 
    private TextMeshProUGUI alterInstructions;

    Vector3 leftDetect;
    Vector3 rightDetect;

    private Renderer rend;
    

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        
        HMDTracker = VRCamera.transform.position.z;
        plankStart = GetComponent<Renderer>().bounds.min.z /* + 5 */;
        plankEnd = GetComponent<Renderer>().bounds.max.z;
        
        Debug.Log("start " + plankStart);
        Debug.Log("end " + plankEnd);

        alterInstructions = Instructions.GetComponent<TextMeshProUGUI>();
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
        
        lateralDifference = Mathf.Abs((0 - VRCamera.transform.position.x) + bodyWiggleRoom);
        //Debug.Log(lateralDifference);

        HMDTracker = VRCamera.transform.position.z;
        Debug.Log(PlankChange2.startToMonitor);

        if(PlankChange2.startToMonitor){
            Fallen();
            monitorSuccessfulTrial(HMDTracker);
        }

        /* if(SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.Any)){
            Debug.Log("Squeezing");
        }else{
            Debug.Log("Not Squeezing");
        } */
        

    }

    void monitorSuccessfulTrial(float userCenter) {
        if(userCenter >= plankEnd){
            VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            VRCamera.GetComponent<Camera>().backgroundColor = Color.black;
            City.SetActive(false);
            rend.enabled = false;
            alterInstructions.text = "You have successfully crossed.\n\n Wait to be guided back and press the trigger when you're ready to begin the next trial.";
            actionPrompt.SetActive(false);
            Instructions.SetActive(true);
            

            successfulTrial = true;
        }
    }
    
    void Fallen() {
        if ((lateralDifference > PlankChange2.plankExtent + bodyWiggleRoom) &&  (VRCamera.transform.position.z > plankStart)){
            VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            VRCamera.GetComponent<Camera>().backgroundColor = Color.black;
            City.SetActive(false);
            rend.enabled = false;
            alterInstructions.text = "You have fallen.\n\n Wait to be guided and press the trigger when you're ready to begin the next trial.";
            actionPrompt.SetActive(false);
            Instructions.SetActive(true);
            

            hasFallen = true;
        }
    }
    
}
