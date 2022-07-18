using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToTrialBeginning : MonoBehaviour
{
    /*private Vector3 tempPos;

    private float startPositionX;
    private float startPositionY;
    private float startPositionZ;*/
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    { 
        startPos = this.transform.position;
        /*startPositionX = transform.position.x;
        startPositionY = transform.position.y;
        startPositionZ = transform.position.z;*/
    }

    // Update is called once per frame
    public void resetTrial() 
    {
        /*//Debug.Log("X: " + startPositionX + " Y: " + startPositionY + " Z: " + startPositionZ);
        tempPos = transform.position;
        tempPos.x = startPositionX;
        tempPos.y = startPositionY ;
        tempPos.z = startPositionZ;
        transform.position = tempPos;*/
        Debug.Log("Method called. Current position " + this.transform.position);

        this.transform.position = startPos;
        Debug.Log("position after change: " + this.transform.position);

    }
}
