using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class recordToCSV : MonoBehaviour
{
    string filename = "";
    /*
    [System.Serializable]
    public class Participant{
        public string name;
        public int blockNumber;
        public float plankWidth;
    }
    [System.Serializable]
    public class ParticipantData{
        public Participant[] participant;
    }

    public ParticipantData user = new ParticipantData() */
    // Start is called before the first frame update
    void Start()
    {
        filename = Application.dataPath + "test.csv";
        recordPrePostData("tally",2, (float)7.76,"PreTrialData-Sky.csv");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void recordPrePostData(string name, int blockNum, float width, string filepath){

        /* Debug.Log("Hello");
        filename = Application.dataPath 
        TextWriter tw = new StreamWriter(filepath, true);
        tw.WriteLine(name + "," + blockNum + "," + width); */
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Name, Health, Damage, Defense");
        tw.Close();

        tw = new StreamWriter(filename, true);
        tw.Close();
        
        /* try
        {
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, true)){
                file.WriteLine(name + "," + blockNum + "," + width);
            }
        }
        catch (System.Exception ex)
        {
            
            throw new ApplicationException("This program kaputed :", ex);
        } */
    }
}
