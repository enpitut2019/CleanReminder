using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataSaveClass
{
    #region Static関数
    /// <summary>
    /// データをpathの位置にjson形式で保存する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="path"></param>
    public static void SaveData<T>(T data,string path)
    {
        StreamWriter writer;
 
        string jsonstr = JsonUtility.ToJson (data);
 
        //writer = new StreamWriter(Application.dataPath +"/"+ path+".json", false);
        writer = new StreamWriter(CreateDataPath(path), false);
        writer.Write (jsonstr);
        writer.Flush ();
        writer.Close ();
    }
    /// <summary>
    /// pathの位置のjson形式のデータをT型で取得する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T LoadData<T>(string path)
        where T : new()
    {
        string datastr = "";
        StreamReader reader;
        if(!CheckFile(path)) SaveData(new T(), path);
        //reader = new StreamReader (Application.dataPath + "/savedata.json");
        reader = new StreamReader (CreateDataPath(path));
        datastr = reader.ReadToEnd ();
        reader.Close ();
    
        return JsonUtility.FromJson<T> (datastr);
    }

    /// <summary>
    /// ファイルが存在するかどうかの確認
    /// </summary>
    /// <returns></returns>
    static bool CheckFile(string path){
        //return File.Exists(Application.dataPath + "/savedata.json");
        return File.Exists(CreateDataPath(path));
    }

    /// <summary>
    /// データのパスを生成する関数
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static string CreateDataPath(string path)
    {
#if UNITY_EDITOR
        return Application.dataPath + "/" + path + ".json";
#else
        
        return Application.persistentDataPath + "/" + path + ".json";
#endif
    }
    #endregion


    public void InitData<T>(string path)
        where T : new()
    {
        File.Delete(CreateDataPath(path));
        LoadData<T>(path);
    }
}
