using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Valve.VR;
using TMPro; 
using System; 



public class DetectFall : MonoBehaviour
{
    private float plankEnd, plankStart;
    
    public static bool hasFallen, successfulTrial;

    private float bodyWiggleRoom = 0.165f;

    private float HMDTracker, userStartLateral;
    private float lateralDifference = 0.0f;

    public GameObject VRCamera,City,Instructions,actionPrompt,respawnLeftTrigger,respawnRightTrigger; 
    private TextMeshProUGUI alterInstructions;

    private Renderer rend;
    

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;

        HMDTracker = RotateWithUser.headPos.x;
        plankStart = RotateWithUser.headPos.x - 1; 
        plankEnd = RotateWithUser.headPos.x - 4.65f;
        userStartLateral = RotateWithUser.headPos.z;

        //HMDTracker = RotateWithUser.headPos.x;
        
        Debug.Log("User start " + HMDTracker);
        Debug.Log("Plank start " + plankStart);
        Debug.Log("Plank end " + plankEnd);

        alterInstructions = Instructions.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {   
        lateralDifference = Mathf.Abs((userStartLateral - RotateWithUser.headPos.z) /* + bodyWiggleRoom */);
        lateralDifference = (float)Math.Round(lateralDifference, 3);
        Debug.Log("Extent: " +  (PlankChange2.plankExtent + bodyWiggleRoom) + " | lateral Diff: " + lateralDifference);
        HMDTracker = RotateWithUser.headPos.x;
        //Debug.Log(HMDTracker);

        //Debug.Log("Extent: " + PlankChange2.plankExtent + " | wiggle room: " + bodyWiggleRoom);
        //Debug.Log("Extent: " +  (PlankChange2.plankExtent + bodyWiggleRoom) + " | lateral Diff: " + lateralDifference);

        if(PlankChange2.startToMonitor){
            Fallen();
            monitorSuccessfulTrial(HMDTracker);
        }

    }

    void monitorSuccessfulTrial(float userCenter) {
        if(userCenter <= plankEnd){
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
        if ((lateralDifference > (PlankChange2.plankExtent + bodyWiggleRoom)) &&  (HMDTracker < plankStart)){

           /*  Debug.Log("User Pos: " + HMDTracker);
            Debug.Log("Plank Start: " + plankStart);
            Debug.Log("Lateral Difference: " + lateralDifference);
            Debug.Log("Available Extent: " + PlankChange2.plankExtent + bodyWiggleRoom); */

            Debug.Log("Extent: " +  (PlankChange2.plankExtent + bodyWiggleRoom) + " | lateral Diff: " + lateralDifference);

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
