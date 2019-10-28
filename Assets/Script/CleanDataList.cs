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
    
    [SerializeField] SEDataTime lastUpdateTime;
    [SerializeField] SEDataTime cleanInterval;
    [SerializeField] string dateData;
    public string DateDataText { get { return dateData; } }
    [SerializeField] string cleanIntervalText;
    public string CleanIntervalText { get { return cleanIntervalText; } }
    
    public CleanPlaceData(string place)
    {
        this.place = place;
        lastUpdateTime = new SEDataTime(DateTime.Now);
        dateData = lastUpdateTime.EntryDate();
        cleanInterval = new SEDataTime();
        cleanIntervalText = cleanInterval.DayInterval();
    }

    public void SetCleanInterval_day(int i)
    {
        cleanInterval.ChengeData_day(i);
        cleanIntervalText = cleanInterval.DayInterval();
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

    public SEDataTime()
    {
        this.year = 0;
        this.month = 0;
        this.day = 0;
        this.hour = 0;
        this.minute = 0;
        this.second = 0;
    }

    public string EntryDate()
    {
        return  this.month + "月" + this.day + "日" + this.hour + "時" + this.minute + "分" + this.second + "秒"; 
    }
    public string DayInterval()
    {
        string Result="";
        if(this.year != 0)
        {
            Result += year + "年";
        }
        if (this.month != 0)
        {
            Result += month + "月";
        }
        if (this.day != 0)
        {
            Result += day + "日";
        }
        if (this.hour != 0)
        {
            Result += hour + "時";
        }
        if (this.minute != 0)
        {
            Result += minute + "分";
        }
        if (this.second != 0)
        {
            Result += second + "秒";
        }
        if(Result == "")
        {
            return "0秒";
        }
        return Result;
    }

    public void ChengeData_day(int i)
    {
        day = i;
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
        return placeDataList[index].DateDataText;
    }

    /// <summary>
    /// CleanPlaceDataを丸ごと取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public CleanPlaceData GetCleanPlaceData(int index)
    {
        return placeDataList[index];
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
