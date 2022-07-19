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

    public Renderer rend;
    private DetectFall DetectFallScript;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;

        DetectFallScript = GetComponent<DetectFall>();

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

        if(Input.GetKeyDown("d")){
            Debug.Log("Script Disabled");
            DetectFallScript.enabled = false;
        }

        if(Input.GetKeyDown("e")){
            Debug.Log("Script Enabled");
            DetectFallScript.enabled = true;
        }

        // //change getKeyDown to space
        // if (Input.GetKeyDown("s") && trialNumber <= 10) {

        // }
        
        // if (DetectFall.successfulTrial && trialNumber <= 10) {
 
        // }
        
        if (DetectFall.hasFallen && trialNumber <= 10) {
            Debug.Log("if statement reached");
            StartCoroutine(prepNextTrials());
        }

        //     assignNewPlankWidth(trialPlankWidths);
        //     temp = transform.localScale;
        //     temp.x = curPlankWidth;
        //     transform.localScale = temp;
        //     ++trialNumber;

        //     WaitUntil(SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any)){

        //     }
        //         Debug.Log("Loop reached");

        
        //             Debug.Log("Trigger Pressed. Reactivating Scene");

        //             VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        //             City.SetActive(true);
        //             rend.enabled = true;

        //             User.transform.position = startPos;

        //             DetectFall.hasFallen = false;
        //             readyToProceed = true;
        //         }else{
        //             continue;
        //         }
        //     }   
        // }

        
        if (trialNumber >= 11) {
            SetUpGroundTrials();
        }
    }

    // private void prepNextTrials(){
    //     Debug.Log("Method reached");

    //     assignNewPlankWidth(trialPlankWidths);
    //     temp = transform.localScale;
    //     temp.x = curPlankWidth;
    //     transform.localScale = temp;
    //     ++trialNumber;
    IEnumerator prepNextTrials()
    {
        Debug.Log("Waiting for user...");
        yield return new WaitUntil(() => SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any));
        Debug.Log("Trigger Pressed. Reactivating Scene");

        assignNewPlankWidth(trialPlankWidths);
        temp = transform.localScale;
        temp.x = curPlankWidth;
        transform.localScale = temp;
        ++trialNumber;

        VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        City.SetActive(true);
        rend.enabled = true;

        Debug.Log("Script Enabled");
        DetectFallScript.enabled = true;
        
        Debug.Log("User is ready!");
    }

    //     while (readyToProceed == false){
    //         Debug.Log("Loop reached");

    //         if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any)){
    //             Debug.Log("Trigger Pressed");
    //             readyToProceed = true;
    //             continue;
    //         }
    //         Debug.Log("Body of code reached");

    //         VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
    //         City.SetActive(true);
    //         rend.enabled = true;

    //         User.transform.position = startPos;

    //         DetectFall.hasFallen = false;
    //     }    
        
    // }

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
