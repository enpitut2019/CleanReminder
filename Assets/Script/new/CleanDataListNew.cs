using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 掃除場所のデータを保持するクラス
/// </summary>
[System.Serializable]
public class CleanDataListNew
{
    /// <summary>
    /// 場所のデータ
    /// </summary>
    public List<string> placeList = new List<string>();

    public void AddPlaceList(string placename)
    {
        placeList.Add(placename);
    }

    public string GetPlaceData(int index)
    {
        return placeList[index];
    }

    public void RemoveData(int index)
    {
        placeList.RemoveAt(index);
    }
}
