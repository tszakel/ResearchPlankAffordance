using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random=UnityEngine.Random;
using UnityEngine.SceneManagement;
using Valve.VR;
using System.Linq;



public class PreTest : MonoBehaviour
{
    private Vector3 temp;
    public float maxPlankWidth = 1.00f;
    public float minPlankWidth = 0.05f;
    private float curPlankWidth;
    List<float> plankWidths = new List<float>();

    public GameObject introScreen, YesNoQuestion, BufferText;

    bool coroutineDisableScreenStarted = false;

    bool coroutineUserInputStarted = false;
    bool coroutineGenerateNextStarted = false;


    // Start is called before the first frame update
    void Start()
    {
        curPlankWidth = minPlankWidth;
        while (curPlankWidth <= maxPlankWidth) {
            plankWidths.Add(curPlankWidth);
            plankWidths.Add(curPlankWidth);
            plankWidths.Add(curPlankWidth);

            curPlankWidth += 0.05f;
            curPlankWidth = (float)Math.Round(curPlankWidth, 2);
        }
    }

    // Update is called once per frame
    void Update(){
        if(introScreen.activeSelf){
            if(!coroutineDisableScreenStarted){
                StartCoroutine(disableIntroScreen());
            }
        }else{
            runPreTest();
        }
    }
    
    IEnumerator disableIntroScreen(){
        Debug.Log("Waiting for user...");

        coroutineDisableScreenStarted = true;
        yield return new WaitUntil(() => /* SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) */ Input.GetKeyDown("c") == true);
        introScreen.SetActive(false);
        YesNoQuestion.SetActive(true);
        assignNewPlankWidth(plankWidths);
    }

    private void runPreTest(){
        Debug.Log("Now running pretest");
        if(!coroutineUserInputStarted){
            StartCoroutine(collectUserInput());
        }
    }

    IEnumerator collectUserInput(){
        coroutineUserInputStarted = true;
        yield return new WaitUntil(() => /* SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) */ Input.GetKeyDown("n") == true || Input.GetKeyDown("y") == true);
        if(Input.GetKeyDown("y")){
            Debug.Log("Fuck it, we ball");
        }else if(Input.GetKeyDown("n")){
            Debug.Log("Hell nah buck-o");
        }    

        YesNoQuestion.SetActive(false);
        BufferText.SetActive(true);

        if(!coroutineGenerateNextStarted){
            StartCoroutine(GenerateNext());
        }
    }

     IEnumerator GenerateNext(){
        coroutineGenerateNextStarted = true;
        yield return new WaitForSeconds(5);
        assignNewPlankWidth(plankWidths);
        YesNoQuestion.SetActive(true);
        BufferText.SetActive(false);

        coroutineUserInputStarted = false;
        coroutineGenerateNextStarted = false;
    }

    private void assignNewPlankWidth(List<float> list)
    {
        int widthListIndex;

        if (list.Any()){
            widthListIndex = Random.Range(0, list.Count);
            curPlankWidth = list[widthListIndex];

            temp = transform.localScale;
            temp.x = curPlankWidth;
            transform.localScale = temp;

            Debug.Log("New Plank Generated " + curPlankWidth);
            list.RemoveAt(widthListIndex);
        }else{
            Debug.Log("You have seen all plank widths");
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
}
