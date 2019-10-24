using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
/// <summary>
/// 掃除場所のデータと掃除していない時間のデータを保持するクラス
/// </summary>
public class CleanPlaceData
{
    [SerializeField] string place;
    public string Place { get { return place; }private set { place = value; } }
    DateTime time;
    [SerializeField] SEDataTime seDate;
    [SerializeField] string dateData;
    public string DateData { get { return dateData; } }
    public DateTime Time { get { return time; } }
    
    public CleanPlaceData(string place)
    {
        this.place = place;
        this.time = DateTime.Now;
        seDate = new SEDataTime(time);
        dateData = seDate.EntryDate();
    }
}
[System.Serializable]
public class SEDataTime{
    [SerializeField] int year;
    [SerializeField] int month;
    [SerializeField] int day;
    [SerializeField] int hour;
    [SerializeField] int minute;
    [SerializeField] int second;

    public SEDataTime(DateTime time)
    {
        this.year = time.Year;
        this.month = time.Month;
        this.day = time.Day;
        this.hour = time.Hour;
        this.minute = time.Minute;
        this.second = time.Second;
    }

    public string EntryDate()
    {
        return this.year + "年" + this.month + "月" + this.day + "日" + this.hour + "時" + this.minute + "分" + this.second + "秒"; 
    }
}

/// <summary>
/// 掃除場所のデータを保持するクラス
/// </summary>
[System.Serializable]
public class CleanDataList
{

    /// <summary>
    /// 場所のデータ
    /// </summary>
    public List<CleanPlaceData> placeDataList = new List<CleanPlaceData>();


    /// <summary>
    /// 場所のデータの追加
    /// </summary>
    /// <param name="placename"></param>
    public void AddPlaceList(string placename)
    {
        //var data = new CleanPlaceData();
        //placeDataList.Add(placename);
        placeDataList.Add(new CleanPlaceData(placename));
    }

    /*public void AddPlaceList(CleanPlaceData data)
    {
        
    }*/

    /// <summary>
    /// 場所のデータの取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetPlaceData(int index)
    {
        return placeDataList[index].Place;
    }

    public string GetDateData(int index)
    {
        return placeDataList[index].DateData;
    }

    /// <summary>
    /// 場所のデータの削除
    /// </summary>
    /// <param name="index"></param>
    public void RemoveData(int index)
    {
        if (placeDataList.Count > index)
        {
            placeDataList.RemoveAt(index);
        }
    }


}
