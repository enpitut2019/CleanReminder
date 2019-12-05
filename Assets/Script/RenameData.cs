using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenameData : MonoBehaviour
{
    CleanPlaceData myData;
    [SerializeField] InputField placeName;

    /// <summary>
    /// 表示するデータの登録
    /// </summary>
    /// <param name="data"></param>
    public void SetRenameData(CleanPlaceData data)
    {
        myData = data;
    }

    /// <summary>
    /// データをInputFieldに表示する関数
    /// </summary>
    public void RenameName()
    {
        if (myData != null)
        {
            placeName.text = myData.Place;
        }
    }
}