using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Valve.VR;
using UnityEngine.SceneManagement;



public class PlankChange2 : MonoBehaviour
{
    private Vector3 temp;
    public float maxPlankWidth = 1.00f;
    public float minPlankWidth = 0.05f;
    private float curPlankWidth;
    public static float plankExtent;


    public GameObject scalePrompt, reachedLimitPrompt, YesNoQuestion, BufferText, actionPrompt, VRCamera, City,City2,City3,City4,City5,City6, User, Instructions;
    private TextMeshProUGUI alterYesNo, alterLimitReached, alterScalePrompt, alterInstructions;

    private int blockNumber = 1;
    public int maxTrials;

    bool responseCoroutineStarted, scaleCoroutineStarted, generateNextCoroutineStarted, limitReachedCoroutineStarted, actionCoroutineStarted,resetParticipantCoroutineStarted, scaleActive;

    public recordToCSV recordtocsv;
    public string userName;
    public bool skyOrGround; 
    public static bool startToMonitor;

    private Renderer rend; //change to public
    private DetectFall DetectFallScript;
    //public RotateWithUser RotateWithUserScript;



    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        DetectFallScript = GetComponent<DetectFall>();
        curPlankWidth = minPlankWidth;
        plankExtent = Math.Abs(curPlankWidth / 2.0f);

        temp = transform.localScale;
        temp.x = minPlankWidth;
        transform.localScale = temp;

        alterYesNo = YesNoQuestion.GetComponent<TextMeshProUGUI>();
        alterLimitReached = reachedLimitPrompt.GetComponent<TextMeshProUGUI>();
        alterScalePrompt = scalePrompt.GetComponent<TextMeshProUGUI>();

        YesNoQuestion.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(blockNumber > maxTrials){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if(YesNoQuestion.activeSelf){
            if(!responseCoroutineStarted){
                StartCoroutine(waitForResponse());
            }
        }

      /*   if(scaleActive && Input.GetKey("s") || scaleActive && SteamVR_Actions._default.SnapTurnLeft.GetState(SteamVR_Input_Sources.Any)){
            if((transform.localScale.x >= minPlankWidth) && (transform.localScale.x <= maxPlankWidth)){
                //Debug.Log("Scaling down");
                temp = transform.localScale;
                temp.x -= 0.005f;
                curPlankWidth -=0.005f;
                plankExtent = Math.Abs(curPlankWidth / 2.0f);
                transform.localScale = temp;
            }
            if(transform.localScale.x <= minPlankWidth){
                alterLimitReached.text = "You have reached the minimum plank width. Please stand by.";
                if(!limitReachedCoroutineStarted){
                    StartCoroutine(plankLimitReached());
                }
            }
        }

        if(scaleActive && Input.GetKey("u") || scaleActive && SteamVR_Actions._default.SnapTurnRight.GetState(SteamVR_Input_Sources.Any)){
            if((transform.localScale.x >= minPlankWidth) && (transform.localScale.x <= maxPlankWidth)){
                //Debug.Log("Scaling up");
                temp = transform.localScale;
                temp.x +=0.005f;
                curPlankWidth +=0.005f;
                plankExtent = Math.Abs(curPlankWidth / 2.0f);
                transform.localScale = temp;
            }
            if(transform.localScale.x >= maxPlankWidth){
                alterLimitReached.text = "You have reached the maximum plank width. Please stand by.";
                if(!limitReachedCoroutineStarted){
                    StartCoroutine(plankLimitReached());
                }
            }
        } */

        

        if(scaleActive && Input.GetKey("s") || scaleActive && SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.Any)){
            if(blockNumber%2 == 1 && transform.localScale.x < maxPlankWidth){
                temp = transform.localScale;
                temp.x += 0.005f;
                curPlankWidth +=0.005f;
                //temp.x = (float)Math.Round(temp.x, 3);
                transform.localScale = temp;
                plankExtent = Math.Abs(curPlankWidth / 2.0f);
            }
            if(blockNumber%2 == 1 && transform.localScale.x >= maxPlankWidth){
                temp = transform.localScale;
                temp.x = minPlankWidth;
                curPlankWidth = minPlankWidth;
                plankExtent = Math.Abs(curPlankWidth / 2.0f);
                transform.localScale = temp;
                /* alterLimitReached.text = "You have reached the maximum plank width.";
                scaleActive = false;
                scaleCoroutineStarted = false;
                responseCoroutineStarted = false;
                if(!limitReachedCoroutineStarted){
                    StartCoroutine(plankLimitReached());
                } */
            }

            if(blockNumber%2 == 0 && transform.localScale.x > minPlankWidth){
                temp = transform.localScale;
                temp.x -= 0.005f;
                curPlankWidth -=0.005f;
                plankExtent = Math.Abs(curPlankWidth / 2.0f);
                //temp.x = (float)Math.Round(temp.x, 3);
                transform.localScale = temp;
            }
            if(blockNumber%2 == 0 && transform.localScale.x <= minPlankWidth){
                temp = transform.localScale;
                temp.x = maxPlankWidth;
                curPlankWidth = maxPlankWidth;
                plankExtent = Math.Abs(curPlankWidth / 2.0f);
                transform.localScale = temp;
                /* alterLimitReached.text = "You have reached the minimum plank width.";
                scaleActive = false;
                scaleCoroutineStarted = false;
                responseCoroutineStarted = false;
                if(!limitReachedCoroutineStarted){
                    StartCoroutine(plankLimitReached());
                } */
            }
        } 

        if(Input.GetKeyDown("d") && scaleActive || SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any) && scaleActive){
            scaleActive = false;
            scalePrompt.SetActive(false);
            YesNoQuestion.SetActive(true);
            responseCoroutineStarted = false;
            scaleCoroutineStarted = false;
        }
    }

    IEnumerator waitForResponse(){
        Debug.Log("Waiting for response...");

        scalePrompt.SetActive(false);
        actionPrompt.SetActive(false);
        reachedLimitPrompt.SetActive(false);
        responseCoroutineStarted = true;
        yield return new WaitUntil(() => SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) || SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any) ||
                                     Input.GetKeyDown("n") == true || Input.GetKeyDown("y") == true);
        if(Input.GetKeyDown("y") == true || SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any) ){
            if(!actionCoroutineStarted){
                StartCoroutine(actionStandBy());
            }
            responseCoroutineStarted = false;
        }else{
            YesNoQuestion.SetActive(false);
            if(blockNumber%2 == 1){
                alterScalePrompt.text = "Scale the width until you feel comfortable walking across.\n Hold the trigger to scale the plank.\n Squeeze side buttons when done";
                scalePrompt.SetActive(true);
            }else{
                alterScalePrompt.text = "Scale the width until you *DONT* feel comfortable walking across.\n Hold the trigger to scale the plank.\n Squeeze side buttons when done";
                scalePrompt.SetActive(true);
            }
            if(!scaleCoroutineStarted){
                StartCoroutine(waitToScale());
            }
        }        
    }


    IEnumerator waitToScale(){
        Debug.Log("Waiting to scale...");
        scaleCoroutineStarted = true;

        yield return new WaitUntil(() => SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.Any) || Input.GetKey("s") == true);

        scaleActive = true;
        scaleCoroutineStarted = false;
    }

    IEnumerator plankLimitReached(){
        limitReachedCoroutineStarted = true;
        scalePrompt.SetActive(false);
        reachedLimitPrompt.SetActive(true);

        yield return new WaitForSeconds(3);

        reachedLimitPrompt.SetActive(false);
        if(!actionCoroutineStarted){
            StartCoroutine(actionStandBy());
        }

        limitReachedCoroutineStarted = false;
    }

     IEnumerator actionStandBy(){
        Debug.Log("Waiting for trial result...");
        scalePrompt.SetActive(false);
        reachedLimitPrompt.SetActive(false);
        YesNoQuestion.SetActive(false);
        actionPrompt.SetActive(true);

        actionCoroutineStarted = true;
        startToMonitor = true;
        yield return new WaitUntil(() => DetectFall.hasFallen == true || DetectFall.successfulTrial == true);

        if(DetectFall.hasFallen == true){
            recordtocsv.recordMainTrialData(userName,blockNumber,transform.localScale.x,DetectFall.hasFallen,skyOrGround);

            DetectFall.hasFallen = false;
            DetectFallScript.enabled = false;

            if(!resetParticipantCoroutineStarted){
                StartCoroutine(resetParticipant());
            }

        }else if(DetectFall.successfulTrial == true){
            recordtocsv.recordMainTrialData(userName,blockNumber,transform.localScale.x,DetectFall.hasFallen,skyOrGround);

            DetectFall.successfulTrial = false;
            DetectFallScript.enabled = false;

            if(!resetParticipantCoroutineStarted){
                StartCoroutine(resetParticipant());
            }
        }
        actionCoroutineStarted = false;
    }

    IEnumerator resetParticipant(){
        resetParticipantCoroutineStarted = true;
        yield return new WaitUntil(() => SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) || Input.GetKeyDown("r") == true);
        Instructions.SetActive(false);
        if(!generateNextCoroutineStarted){
            StartCoroutine(GenerateNext());
        }
       
        resetParticipantCoroutineStarted = false;
    }

    IEnumerator GenerateNext(){
        generateNextCoroutineStarted = true;

        reachedLimitPrompt.SetActive(false);
        BufferText.SetActive(true);
        startToMonitor = false;

        yield return new WaitForSeconds(5);
        

        if(blockNumber%2 == 0){
            temp = transform.localScale;
            temp.x = minPlankWidth;
            curPlankWidth = minPlankWidth;
            plankExtent = Math.Abs(curPlankWidth / 2.0f);
            transform.localScale = temp;
            ++blockNumber;

            alterYesNo.text = "Is this a width you feel comfortable walking across?\n Squeeze side buttons for 'Yes'\n Trigger for 'No'";
        }else{
            temp = transform.localScale;
            temp.x = maxPlankWidth;
            curPlankWidth = maxPlankWidth;
            plankExtent = Math.Abs(curPlankWidth / 2.0f);
            transform.localScale = temp;
            ++blockNumber;

            alterYesNo.text = "Is this the *SMALLEST* width you feel comfortable walking across?\n Squeeze side buttons for 'Yes'\n Trigger for 'No'";
        }

        //RotateWithUserScript.updateEnvironment();

        VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        City.SetActive(true);
        City2.SetActive(true);
        City3.SetActive(true);
        City4.SetActive(true);
        City5.SetActive(true);
        City6.SetActive(true);
        rend.enabled = true;

        BufferText.SetActive(false);
        actionPrompt.SetActive(false);
        YesNoQuestion.SetActive(true);
        DetectFallScript.enabled = true;

        generateNextCoroutineStarted = false;
    }
}
