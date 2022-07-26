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
    string filePath, newEntry;
    string DirectoryPath1;

    //FileStream file = File.Create(Application.persistentDataPath + "/" + foldername + "/" + filesavename + fileextnison);
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
        DirectoryPath1 = Application.dataPath + "/ExperimentData";
        //recordPrePostData("tally", 2, (float)7.76);
        //recordPrePostData("tally", 3, (float)3.44);

        //Debug.Log("Data path: "+ DirectoryPath1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void recordMainTrialData(string name, int blockNum, float width, string skyOrGround){
        filePath = DirectoryPath1 + "/" + name + ".csv";
        if(!File.Exists(filePath)){
            using (StreamWriter sw = File.CreateText(filePath)){
                sw.WriteLine("Name, Block #, Width");
            }
        }else{
            using (StreamWriter sw = File.AppendText(filePath)){
                newEntry = "" + name + "," + blockNum + "," + width;
                sw.WriteLine(newEntry);
            }
        }
    }

    public void recordPrePostData(string name, int blockNum, float width, string skyOrGround){
        filePath = DirectoryPath1 + "/" + name + ".csv";
        if(!File.Exists(filePath)){
            using (StreamWriter sw = File.CreateText(filePath)){
                sw.WriteLine("Name, Block #, Width");
            }
        }else{
            using (StreamWriter sw = File.AppendText(filePath)){
                newEntry = "" + name + "," + blockNum + "," + width;
                sw.WriteLine(newEntry);
            }
        }
    }
}
