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
    
    public GameObject VRCamera,City,User,Instructions;

    bool coroutineStarted = false;

    Vector3 startPos;

    public Renderer rend;
    private DetectFall DetectFallScript;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;

        DetectFallScript = GetComponent<DetectFall>();

        temp = transform.localScale;
        temp.x = maxPlankWidth;
        transform.localScale = temp;
        
        plankExtent = (maxPlankWidth - minPlankWidth) / 2.0f;
        
        for (int i = 0; i < 10; ++i) {
            maxPlankWidth -= 0.1f;
            maxPlankWidth = (float)Math.Round(maxPlankWidth, 2);
            //Debug.Log("Plank " + i + ": " + maxPlankWidth);
            trialPlankWidths.Add(maxPlankWidth);
        }

        
        Debug.Log("plank extent: " + plankExtent);

    }

    // Update is called once per frame
    void Update()
    {
        // //change getKeyDown to space
        // if (Input.GetKeyDown("s") && trialNumber <= 10) {

        // }
        
        // if (DetectFall.successfulTrial && trialNumber <= 10) {
 
        // }
        
        if (DetectFall.hasFallen && trialNumber <= 10) {
            Debug.Log("if statement reached");
            DetectFall.hasFallen = false;
            DetectFallScript.enabled = false;

            //if anything funky happens it could be in this if block
            if(!coroutineStarted){
                StartCoroutine(prepNextTrials());
            }
            
        }

        
        if (trialNumber >= 11) {
            SetUpGroundTrials();
        }
    }

    IEnumerator prepNextTrials()
    {
        Debug.Log("Waiting for user...");

        coroutineStarted = true;
        assignNewPlankWidth(trialPlankWidths);
        temp = transform.localScale;
        temp.x = curPlankWidth;
        transform.localScale = temp;
        ++trialNumber;

        yield return new WaitUntil(() => SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any)/*  Input.GetKeyDown("n") == true */);
        Debug.Log("Trigger Pressed. Reactivating Scene");


        VRCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        City.SetActive(true);
        Instructions.SetActive(false);
        rend.enabled = true;

        DetectFallScript.enabled = true;
        coroutineStarted = false;
        
        Debug.Log("User is ready!");
    }

    private void assignNewPlankWidth(List<float> list)
    {
        int widthListIndex;
        if (list.Any()){
            widthListIndex = Random.Range(0, list.Count);
            curPlankWidth = list[widthListIndex];
            plankExtent = Math.Abs((curPlankWidth - minPlankWidth) / 2.0f);
            Debug.Log("new plank extent: " + plankExtent);

            list.RemoveAt(widthListIndex);
        }
    }
    
    void SetUpGroundTrials() {
        trialNumber = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
