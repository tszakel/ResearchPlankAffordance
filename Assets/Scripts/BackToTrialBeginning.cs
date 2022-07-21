using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToTrialBeginning : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            player.transform.position = respawnPoint.transform.position;
            Physics.SyncTransforms();
        }
    }

   /*  private Vector3 tempPos;
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
