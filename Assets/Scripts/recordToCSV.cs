using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class recordToCSV : MonoBehaviour
{
    string filePath, newEntry;
    string DirectoryPathSky;
    string DirectoryPathGround;

    // Start is called before the first frame update
    void Start()
    {
        DirectoryPathSky = Application.dataPath + "/ExperimentData/SkyData";
        DirectoryPathGround = Application.dataPath + "/ExperimentData/GroundData";
    }


    public void recordMainTrialData(string name, int blockNum, float width, bool fall, bool skyOrGround){
        if(skyOrGround){
            filePath = DirectoryPathSky + "/" + name + "-sky.csv";
        }else{
            filePath = DirectoryPathGround + "/" + name + "-ground.csv";       
        }

        if(fall){
            newEntry = "" + blockNum + "," + width + ", Yes";
        }else{
            newEntry = "" + blockNum + "," + width + ", No";
        }
        

        if(blockNum == 1){
            using (StreamWriter sw = File.AppendText(filePath)){
                sw.WriteLine("MainTrials");
                sw.WriteLine("Block #, Plank Width, Fall?");
                sw.WriteLine(newEntry);
            }
        }else{
            using (StreamWriter sw = File.AppendText(filePath)){
                sw.WriteLine(newEntry);
            }
        }
    }

    public void recordPrePostData(string name, int blockNum, float width, bool skyOrGround, bool preOrPost){
        if(skyOrGround){
            filePath = DirectoryPathSky + "/" + name + "-sky.csv";
        }else{
            filePath = DirectoryPathGround + "/" + name + "-ground.csv";
        }   

        newEntry = "" + blockNum + "," + width;

        if(blockNum == 1 && preOrPost){
            using (StreamWriter sw = File.AppendText(filePath)){
                sw.WriteLine("PreTest");
                sw.WriteLine("Block #, Plank Width");
                sw.WriteLine(newEntry);
            }
        }else if(blockNum == 1 && !preOrPost){
            using (StreamWriter sw = File.AppendText(filePath)){
                sw.WriteLine("PostTest");
                sw.WriteLine("Block #, Plank Width");
                sw.WriteLine(newEntry);
            }
        }else{
            using (StreamWriter sw = File.AppendText(filePath)){
                sw.WriteLine(newEntry);
            }
        }    
    }
}
