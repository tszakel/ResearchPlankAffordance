using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random=UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Valve.VR;

public class PlankChange : MonoBehaviour
{
    private Vector3 temp;
    public float maxPlankWidth = 1.45f;
    public float minPlankWidth = 0.55f;
    private float curPlankWidth;
    List<float> trialPlankWidths = new List<float>();
    private int trialNumber = 1;
    public static float plankExtent;
    
    public GameObject VRCamera,City,User;

    Vector3 startPos;

    //public BackToTrialBeginning resetTrialScript;
    
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = User.transform.position;
        Debug.Log("Start Position: " + startPos);

        temp = transform.localScale;
        temp.x = maxPlankWidth;
        transform.localScale = temp;
        
        plankExtent = (maxPlankWidth - minPlankWidth) / 2.0f;
        
        for (int i = 0; i < 10; ++i) {
            maxPlankWidth -= 0.1f;
            trialPlankWidths.Add(maxPlankWidth);
        }

        
        Debug.Log("plank: " + plankExtent);

    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) || Input.GetKeyDown("g")){
            Debug.Log("Trigger or 'G' has been pressed");
            User.transform.position = new Vector3(0,20,0);
            //resetTrialScript.resetTrial();
        }

        //change getKeyDown to space
        if (Input.GetKeyDown("s") && trialNumber <= 10) {
            prepNextTrials();
            // assignNewPlankWidth(trialPlankWidths);
            // temp = transform.localScale;
            // temp.x = curPlankWidth;
            // transform.localScale = temp; //while change skybox = false
            // VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            // City.SetActive(true);
            // this.gameObject.SetActive(true);

            // ++trialNumber;
        }
        
        if (DetectFall.successfulTrial && trialNumber <= 10) {
            prepNextTrials();
            // assignNewPlankWidth(trialPlankWidths);
            // temp = transform.localScale;
            // temp.x = curPlankWidth;
            // transform.localScale = temp;
            // ++trialNumber;
            // VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            // City.SetActive(true);
            // this.gameObject.SetActive(true);



            // DetectFall.successfulTrial = false;

        }
        
        if (DetectFall.hasFallen && trialNumber <= 10) {
            prepNextTrials();
            // Debug.Log("you have fallen");

            // assignNewPlankWidth(trialPlankWidths);
            // temp = transform.localScale;
            // temp.x = curPlankWidth;
            // transform.localScale = temp;
            // ++trialNumber;
            
            // VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            // City.SetActive(true);
            // this.gameObject.SetActive(true);


            // resetTrialScript.resetTrial();

            // DetectFall.hasFallen = false;
        }
        
        
        if (trialNumber >= 11) {
            SetUpGroundTrials();
        }
    }

    private void prepNextTrials(){
        assignNewPlankWidth(trialPlankWidths);
        temp = transform.localScale;
        temp.x = curPlankWidth;
        transform.localScale = temp;
        ++trialNumber;

        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any)){
            VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            City.SetActive(true);
            this.gameObject.SetActive(true);

            //resetTrialScript.resetTrial();

            DetectFall.hasFallen = false;
        }    
        
    }

    private void assignNewPlankWidth(List<float> list)
    {
        int widthListIndex;
        if (list.Any()){
            widthListIndex = Random.Range(0, list.Count);
            curPlankWidth = list[widthListIndex];
            list.RemoveAt(widthListIndex);
        }
    }
    
    void SetUpGroundTrials() {
        trialNumber = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
