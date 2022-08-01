using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToTrialBeginning : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    /* private void OnTriggerEnter(Collider other){
         if(other.CompareTag("Player")){
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
    }  */

    /* private Vector3 tempPos;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    { 
        startPos = this.transform.position;
        Debug.Log("Cube Start Position: " + startPos);
    }

    void Update(){
        resetTrial();
    }

    // Update is called once per frame
    public void resetTrial() 
    {
        if(Input.GetKeyDown("t")){
            Debug.Log("Transport triggerd. Current position " + this.transform.position);
            this.transform.position = startPos;
            Debug.Log("Position after change: " + this.transform.position);
        }
        
    } */
}
