using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Valve.VR;
using UnityEngine.SceneManagement;



public class PreTest2 : MonoBehaviour
{
    private Vector3 temp;
    public float maxPlankWidth = 1.00f;
    public float minPlankWidth = 0.05f;
    private float curPlankWidth;
    List<float> trialPlankWidths = new List<float>();
    private float recordWidth;

    public GameObject scalePrompt, reachedLimitPrompt, YesNoQuestion, BufferText;
    private TextMeshProUGUI alterYesNo, alterLimitReached, alterScalePrompt;

    private int blockNumber = 1;
    public int maxTrials;


    bool responseCoroutineStarted, scaleCoroutineStarted, generateNextCoroutineStarted, limitReachedCoroutineStarted, scaleActive;

    public recordToCSV recordtocsv;
    public string userName;
    public bool skyOrGround, preOrPost;



    // Start is called before the first frame update
    void Start()
    {
        temp = transform.localScale;
        temp.x = minPlankWidth;
        transform.localScale = temp;

        alterYesNo = YesNoQuestion.GetComponent<TextMeshProUGUI>();
        alterLimitReached = reachedLimitPrompt.GetComponent<TextMeshProUGUI>();
        alterScalePrompt = scalePrompt.GetComponent<TextMeshProUGUI>();

        YesNoQuestion.SetActive(true);

        //Debug.Log("user name: " + userName + ", sky/ground: " + skyOrGround);
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

        if(scaleActive && Input.GetKey("s") || scaleActive && SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.Any)){
            if(blockNumber%2 == 1 && transform.localScale.x < maxPlankWidth){
                temp = transform.localScale;
                temp.x += 0.01f;
                //temp.x = (float)Math.Round(temp.x, 3);
                transform.localScale = temp;
            }
            if(blockNumber%2 == 1 && transform.localScale.x >= maxPlankWidth){
                alterLimitReached.text = "You have reached the maximum plank width. Please stand by.";
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
                alterLimitReached.text = "You have reached the minimum plank width. Please stand by.";
                if(!limitReachedCoroutineStarted){
                    StartCoroutine(plankLimitReached());
                }
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

        responseCoroutineStarted = true;
        yield return new WaitUntil(() => SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) || SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any) ||
                                     Input.GetKeyDown("n") == true || Input.GetKeyDown("y") == true);
        if(Input.GetKeyDown("y") == true || SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any)){
            recordWidth = transform.localScale.x;
            Debug.Log("Width recorded: " + recordWidth);
            if(!generateNextCoroutineStarted){
                StartCoroutine(GenerateNext());
            }
            responseCoroutineStarted = false;
        }else{
            YesNoQuestion.SetActive(false);
            if(blockNumber%2 == 1){
                alterScalePrompt.text = "Scale up the width until you feel comfortable walking across. Squeeze when done";
                scalePrompt.SetActive(true);
            }else{
                alterScalePrompt.text = "Scale down the width until you *DONT* feel comfortable walking across. Squeeze when done";
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

        recordWidth = transform.localScale.x;
        Debug.Log("Width recorded: " + recordWidth);


        limitReachedCoroutineStarted = true;
        scalePrompt.SetActive(false);
        reachedLimitPrompt.SetActive(true);

        yield return new WaitForSeconds(5);

        generateNextCoroutineStarted = false;
        if(!generateNextCoroutineStarted){
            StartCoroutine(GenerateNext());
        }
        responseCoroutineStarted = false;
        limitReachedCoroutineStarted = false;
    }

    IEnumerator GenerateNext(){
        generateNextCoroutineStarted = true;
        scaleActive = false;
        YesNoQuestion.SetActive(false);
        scalePrompt.SetActive(false);
        reachedLimitPrompt.SetActive(false);
        BufferText.SetActive(true);

        yield return new WaitForSeconds(5);

        if(blockNumber%2 == 0){
            //recordtocsv.recordPrePostData(userName,blockNumber,transform.localScale.x,skyOrGround,preOrPost);

            temp = transform.localScale;
            temp.x = minPlankWidth;
            transform.localScale = temp;
            alterYesNo.text = "Is this a width you feel comfortable walking across?\n Squeeze for 'Yes'\n Trigger for 'No'";
        }else{
            //recordtocsv.recordPrePostData(userName,blockNumber,transform.localScale.x,skyOrGround,preOrPost);         

            temp = transform.localScale;
            temp.x = maxPlankWidth;
            transform.localScale = temp;
            alterYesNo.text = "Is this the *smallest* width you feel comfortable walking across?\n Squeeze for 'Yes'\n Trigger for 'No'";
        }

        ++blockNumber;

        BufferText.SetActive(false);
        YesNoQuestion.SetActive(true);

        generateNextCoroutineStarted = false;
    }
}
