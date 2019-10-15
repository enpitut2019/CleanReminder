using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataSave
{

    public void savePlayerData(CleanDataListNew player)
    {
        StreamWriter writer;
 
        string jsonstr = JsonUtility.ToJson (player);
 
        writer = new StreamWriter(Application.dataPath + "/savedata.json", false);
        writer.Write (jsonstr);
        writer.Flush ();
        writer.Close ();
    }

    public CleanDataListNew loadPlayerData()
    {
        string datastr = "";
        StreamReader reader;
        if(!CheckFile())savePlayerData(new CleanDataListNew());
        reader = new StreamReader (Application.dataPath + "/savedata.json");
        datastr = reader.ReadToEnd ();
        reader.Close ();
    
        return JsonUtility.FromJson<CleanDataListNew> (datastr);
    }

    bool CheckFile(){
        return File.Exists(Application.dataPath + "/savedata.json");
    }
}
