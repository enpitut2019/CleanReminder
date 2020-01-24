using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// cleanPlaceDataボタンの表示をするクラス
/// </summary>
public class PlaceButtonLayout : MonoBehaviour
{
    [SerializeField] Text placeText;
    [SerializeField] Text limitText;
    [SerializeField] Colorbar colorbar;

    /// <summary>
    /// データの登録
    /// </summary>
    /// <param name="data"></param>
    public void SetCleanPlaceData(CleanPlaceData data)
    {
        placeText.text = data.Place;
        limitText.text = "あと"+data.NextCleanLeftTimeText;
        colorbar.ChangeColor(data);
    }
}
