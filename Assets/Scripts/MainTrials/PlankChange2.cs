using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Valve.VR;


public class PlankChange2 : MonoBehaviour
{
    private Vector3 temp;
    public float maxPlankWidth = 1.00f;
    public float minPlankWidth = 0.05f;
    private float curPlankWidth;
    List<float> trialPlankWidths = new List<float>();
    private float recordWidth;
    public static float plankExtent;


    public GameObject scalePrompt, reachedLimitPrompt, YesNoQuestion, BufferText, actionPrompt, VRCamera, City, User, Instructions;
    private TextMeshProUGUI alterYesNo, alterLimitReached, alterScalePrompt, alterInstructions;

    private int blockNumber = 1;
    public int maxTrials = 4;

    bool responseCoroutineStarted = false;
    bool scaleCoroutineStarted = false;
    bool generateNextCoroutineStarted = false;
    bool limitReachedCoroutineStarted = false;
    bool actionCoroutineStarted = false;
    bool resetParticipantCoroutineStarted = false;



    bool scaleActive = false;

    public recordToCSV recordtocsv;

    private Renderer rend; //change to public
    private DetectFall DetectFallScript;



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
        if(YesNoQuestion.activeSelf){
            if(!responseCoroutineStarted){
                StartCoroutine(waitForResponse());
            }
        }

        if(scaleActive && Input.GetKey("s")){
            if(blockNumber%2 == 1 && transform.localScale.x < maxPlankWidth){
                temp = transform.localScale;
                temp.x += 0.01f;
                //temp.x = (float)Math.Round(temp.x, 3);
                transform.localScale = temp;
            }
            if(blockNumber%2 == 1 && transform.localScale.x >= maxPlankWidth){
                alterLimitReached.text = "You have reached the maximum plank width. Please stand walk across the plank.";
                if(!limitReachedCoroutineStarted){
                    StartCoroutine(plankLimitReached());
                }
            }

            if(blockNumber%2 == 0 && transform.localScale.x > minPlankWidth){
                temp = transform.localScale;
                temp.x -= 0.01f;
                //temp.x = (float)Math.Round(temp.x, 3);
                transform.localScale = temp;
            }
            if(blockNumber%2 == 0 && transform.localScale.x <= minPlankWidth){
                alterLimitReached.text = "You have reached the minimum plank width. Please stand walk across the plank.";
                if(!limitReachedCoroutineStarted){
                    StartCoroutine(plankLimitReached());
                }
            }
        }

        if(Input.GetKeyDown("d") && scaleActive){
            scaleActive = false;
            scalePrompt.SetActive(false);
            YesNoQuestion.SetActive(true);
            responseCoroutineStarted = false;
            scaleCoroutineStarted = false;
        }
    }

    IEnumerator waitForResponse(){
        Debug.Log("Waiting for response...");

        responseCoroutineStarted = true;
        yield return new WaitUntil(() => /* SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) */ Input.GetKeyDown("n") == true || Input.GetKeyDown("y") == true);
        if(Input.GetKeyDown("y") == true){
            if(!actionCoroutineStarted){
                StartCoroutine(actionStandBy());
            }
            responseCoroutineStarted = false;
            /* if(!generateNextCoroutineStarted){
                StartCoroutine(GenerateNext());
            }
            responseCoroutineStarted = false; */
        }else{
            YesNoQuestion.SetActive(false);
            if(blockNumber%2 == 1){
                alterScalePrompt.text = "Scale up the width until you feel comfortable walking across. Press ... when done";
                scalePrompt.SetActive(true);
            }else{
                alterScalePrompt.text = "Scale down the width until you *DONT* feel comfortable walking across. Press ... when done";
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

        yield return new WaitUntil(() => /* SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) */ Input.GetKey("s") == true);

        scaleActive = true;
        scaleCoroutineStarted = false;
    }

    IEnumerator plankLimitReached(){
        limitReachedCoroutineStarted = true;
        scalePrompt.SetActive(false);
        reachedLimitPrompt.SetActive(true);

        yield return new WaitForSeconds(5);

        if(!actionCoroutineStarted){
            StartCoroutine(actionStandBy());
        }

        limitReachedCoroutineStarted = false;
    }

     IEnumerator actionStandBy(){
        Debug.Log("Waiting for trial result...");

        actionCoroutineStarted = true;
        yield return new WaitUntil(() => DetectFall.hasFallen == true || DetectFall.successfulTrial == true);

        if(DetectFall.hasFallen == true){
            DetectFall.hasFallen = false;
            DetectFallScript.enabled = false;

            if(!resetParticipantCoroutineStarted){
                StartCoroutine(resetParticipant());
            }

        }else if(DetectFall.successfulTrial == true){
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
        yield return new WaitUntil(() => SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) || Input.GetKeyDown("r"));

        VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        City.SetActive(true);
        Instructions.SetActive(false);
        alterInstructions.text = "You have fallen.\n Wait to be guided and press the trigger when you're ready to begin the next trial.";
        rend.enabled = true;

        if(!generateNextCoroutineStarted){
            StartCoroutine(GenerateNext());
        }
       
        resetParticipantCoroutineStarted = false;
    }


    IEnumerator GenerateNext(){
        generateNextCoroutineStarted = true;
        BufferText.SetActive(true);

        yield return new WaitForSeconds(5);

        if(blockNumber%2 == 0){
            //record plank size
            temp = transform.localScale;
            temp.x = minPlankWidth;
            transform.localScale = temp;
            ++blockNumber;

            alterYesNo.text = "Is this a width you feel comfortable walking across?\n Trigger for 'Yes'\n Squeeze for 'No'";
        }else{
            //record plank size
            temp = transform.localScale;
            temp.x = maxPlankWidth;
            transform.localScale = temp;
            ++blockNumber;

            alterYesNo.text = "Is this the *smallest* width you feel comfortable walking across?\n Trigger for 'Yes'\n Squeeze for 'No'";
        }

        BufferText.SetActive(false);
        YesNoQuestion.SetActive(true);
        DetectFallScript.enabled = true;

        generateNextCoroutineStarted = false;
    }
}
