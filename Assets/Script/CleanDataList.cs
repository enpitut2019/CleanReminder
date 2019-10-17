using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 掃除場所のデータを保持するクラス
/// </summary>
[System.Serializable]
public class CleanDataList
{
    /// <summary>
    /// 場所のデータ
    /// </summary>
    public List<string> placeList = new List<string>();

    /// <summary>
    /// 場所のデータの追加
    /// </summary>
    /// <param name="placename"></param>
    public void AddPlaceList(string placename)
    {
        placeList.Add(placename);
    }

    /// <summary>
    /// 場所のデータの取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetPlaceData(int index)
    {
        return placeList[index];
    }

    /// <summary>
    /// 場所のデータの削除
    /// </summary>
    /// <param name="index"></param>
    public void RemoveData(int index)
    {
        if (placeList.Count > index)
        {
            placeList.RemoveAt(index);
        }
    }
}
