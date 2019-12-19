using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetIntervalPanel : MonoBehaviour
{
    CleanPlaceData myData;
    [SerializeField] Text placeName;

    /// <summary>
    /// 表示するデータの登録
    /// </summary>
    /// <param name="data"></param>
    public void SetIntervalData(CleanPlaceData data)
    {
        myData = data;
    }

    /// <summary>
    /// データをInputFieldに表示する関数
    /// </summary>
    public void IntervalPlaceName()
    {
        if (myData != null)
        {
            placeName.text = myData.Place;
        }
    }
}