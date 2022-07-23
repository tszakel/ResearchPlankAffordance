using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreTest2 : MonoBehaviour
{
    private Vector3 temp;
    public float maxPlankWidth = 1.00f;
    public float minPlankWidth = 0.05f;
    private float curPlankWidth;
    List<float> trialPlankWidths = new List<float>();
    private int trialNumber = 1;
    public static float plankExtent;

    public GameObject sizeUpPrompt, sizeDownPrompt, reachedLimitPrompt, YesNoQuestion, BufferText;

    private int blockNumber = 1;

    bool responseCoroutineStarted = false;
    bool scaleCoroutineStarted = false;
    bool generateNextCoroutineStarted = false;
    bool scaleActive = false;



    // Start is called before the first frame update
    void Start()
    {
        temp = transform.localScale;
        temp.x = minPlankWidth;
        transform.localScale = temp;

        YesNoQuestion.SetActive(true);

        //sizeUpPrompt.SetActive(true);        
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
                transform.localScale = temp;
            }
            if(blockNumber%2 == 1 && transform.localScale.x >= maxPlankWidth){
                Debug.Log("Max width reached");
                generateNextCoroutineStarted = false;
                if(!generateNextCoroutineStarted){
                    StartCoroutine(GenerateNext());
                }
                responseCoroutineStarted = false;
            }

            if(blockNumber%2 == 0 && transform.localScale.x > minPlankWidth){
                temp = transform.localScale;
                temp.x -= 0.01f;
                transform.localScale = temp;
            }
            if(blockNumber%2 == 0 && transform.localScale.x <= minPlankWidth){
                Debug.Log("min width reached");
                generateNextCoroutineStarted = false;
                if(!generateNextCoroutineStarted){
                    StartCoroutine(GenerateNext());
                }
                responseCoroutineStarted = false;
            }
        }

        if(Input.GetKeyDown("d") && scaleActive){
            scaleActive = false;
            sizeUpPrompt.SetActive(false);
            sizeDownPrompt.SetActive(false);
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
            if(!generateNextCoroutineStarted){
                StartCoroutine(GenerateNext());
            }
            responseCoroutineStarted = false;
        }else{
            YesNoQuestion.SetActive(false);
            if(blockNumber%2 == 1){
                sizeUpPrompt.SetActive(true);
            }else{
                sizeDownPrompt.SetActive(true);
            }
            if(!scaleCoroutineStarted){
                StartCoroutine(waitToScale());
            }
        }
        
        //YesNoQuestion.SetActive(true);
    }


    IEnumerator waitToScale(){
        Debug.Log("Waiting to scale...");

        scaleCoroutineStarted = true;
        yield return new WaitUntil(() => /* SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) */ Input.GetKey("s") == true);

        scaleActive = true;
        scaleCoroutineStarted = false;
    }

    IEnumerator plankLimitReached(){

        scaleCoroutineStarted = true;
        yield return new WaitUntil(() => /* SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) */ Input.GetKey("s") == true);

        scaleActive = true;
        scaleCoroutineStarted = false;
    }

    IEnumerator GenerateNext(){
        generateNextCoroutineStarted = true;
        scaleActive = false;
        YesNoQuestion.SetActive(false);
        sizeUpPrompt.SetActive(false);
        sizeDownPrompt.SetActive(false);
        BufferText.SetActive(true);

        yield return new WaitForSeconds(5);

        if(blockNumber%2 == 0){
            //record plank size
            temp = transform.localScale;
            temp.x = minPlankWidth;
            transform.localScale = temp;
            ++blockNumber;
        }else{
            //record plank size
            temp = transform.localScale;
            temp.x = maxPlankWidth;
            transform.localScale = temp;
            ++blockNumber;
        }
        BufferText.SetActive(false);
        YesNoQuestion.SetActive(true);

        generateNextCoroutineStarted = false;
    }
}
