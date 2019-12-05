using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemovePanel : MonoBehaviour
{
    CleanPlaceData myData;
    [SerializeField] Text placeName;

    /// <summary>
    /// 表示するデータの登録
    /// </summary>
    /// <param name="data"></param>
    public void SetRemoveData(CleanPlaceData data)
    {
        myData = data;
    }

    /// <summary>
    /// データをテキストに表示する関数
    /// </summary>
    public void RemoveName()
    {
        if (myData != null)
        {
            placeName.text = myData.Place;
        }
    }
}