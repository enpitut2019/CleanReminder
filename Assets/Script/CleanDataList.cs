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
    public SEDataTime LastUpdateTime { get { return lastUpdateTime; } }
    [SerializeField] SEDataTime cleanInterval;
    public SEDataTime CleanInterval { get { return cleanInterval; } }
    [SerializeField] string dateData;
    public string DateDataText { get { return dateData; } }
    [SerializeField] string cleanIntervalText;
    public string CleanIntervalText { get { return cleanIntervalText; } }

    //追加============================
    public DateTime lastUpdateDateTime;
    public TimeSpan cleanTimeSpan;
    public DateTime nextLimitDateTime { get { return lastUpdateDateTime + cleanTimeSpan; } }
    public TimeSpan ElapasedTime { get { return DateTime.Now - lastUpdateDateTime; } }//最後に掃除してからの経過時間
    public TimeSpan LimitTimeSpan { get { return nextLimitDateTime - DateTime.Now; } }
    //=============================
    
    public CleanPlaceData(string place)
    {
        this.place = place;
        /*lastUpdateTime = new SEDataTime(DateTime.Now);
        dateData = lastUpdateTime.EntryDate();
        cleanInterval = new SEDataTime();
        cleanIntervalText = cleanInterval.DayInterval();*/
        lastUpdateDateTime = DateTime.Now;
        cleanTimeSpan = new TimeSpan();
        SetSETime();
        cleanIntervalText = cleanInterval.DayInterval();
    }

    public void SetCleanIntervalDate(int i)
    {
        cleanInterval.ChangeDate(i);
        cleanIntervalText = cleanInterval.DayInterval();
    }

    /// <summary>
    /// DateTime->SEへの変換
    /// </summary>
    void SetSETime()
    {
        lastUpdateTime =new SEDataTime(lastUpdateDateTime);
        cleanInterval = new SEDataTime(cleanTimeSpan);
    }

    /// <summary>
    /// SE->DateTImeへの変換
    /// </summary>
    void SetDateTime()
    {
        lastUpdateDateTime = TimeCalucurator.REDataTime(lastUpdateTime);
        cleanTimeSpan = TimeCalucurator.ReTimeSpan(cleanInterval);
    }

    
    public void ChangeCleanTimeSpan(int num)
    {
        cleanInterval.ChangeDate(num);
        SetDateTime();
    }

    public void InitAction()
    {
        SetDateTime();
    }
}

public class TimeCalucurator
{
    public static TimeSpan GetTimeSpan(DateTime frontTime,DateTime backTime)
    {
        return frontTime - backTime;
    }
    public static DateTime REDataTime(SEDataTime time)
    {
        var datas = time.OutDayDatas();
        DateTime reDataTime = new DateTime(datas[0],datas[1],datas[2],datas[3],datas[4],datas[5]);
        return reDataTime;
    }

    public static TimeSpan ReTimeSpan(SEDataTime time)
    {
        var datas = time.OutDayDatas();
        TimeSpan reDataTime = new TimeSpan( datas[2], datas[3], datas[4], datas[5]);
        return reDataTime;
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
    string targetkey="";
    Dictionary<string,int> dataTimeDictionary;
    


    public SEDataTime(DateTime time)
    {
        this.year = time.Year;
        this.month = time.Month;
        this.day = time.Day;
        this.hour = time.Hour;
        this.minute = time.Minute;
        this.second = time.Second;
    }

    public SEDataTime(TimeSpan time)
    {
        this.year = 0;
        this.month = 0;
        this.day = time.Days;
        this.hour = time.Hours;
        this.minute = time.Minutes;
        this.second = time.Seconds;
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

    public bool ChangeTarget(string key)
    {
        if (dataTimeDictionary == null)
        {
            InitDictionary();
        }
        if (dataTimeDictionary.ContainsKey(key))
        {
            targetkey = key;
            return true;
        }
        return false;
    }

    public void ChangeDate(int value)
    {
        if (dataTimeDictionary == null)
        {
            InitDictionary();
        }
        if (dataTimeDictionary.ContainsKey(targetkey))
        {
            //dataTimeDictionary[targetkey] = value;
            switch (targetkey)
            {
                case "Year":
                    year = value;
                    break;
                case "Month":
                    month = value;
                    break;
                case "Day":
                    day = value;
                    break;
                case "Hour":
                    hour = value;
                    break;
                case "Minute":
                    minute = value;
                    break;
                case "Second":
                    second = value;
                    break;
            }
        }
    }

    public void ChangeDate(string key,int value)
    {
        ChangeTarget(key);
        ChangeDate(value);
    }

    public int GetDate(string key)
    {
        if(dataTimeDictionary == null)
        {
            InitDictionary();
        }
        int result = -1;
        if (dataTimeDictionary.ContainsKey(key))
        {
            result = dataTimeDictionary[key];
        }
        return result;
    }

    void InitDictionary()
    {
        dataTimeDictionary = new Dictionary<string, int>();
        dataTimeDictionary.Add("Year", year);
        dataTimeDictionary.Add("Month", month);
        dataTimeDictionary.Add("Day", day);
        dataTimeDictionary.Add("Hour", hour);
        dataTimeDictionary.Add("Minute", minute);
        dataTimeDictionary.Add("Second", second);
    }

    //時間に換算する関数
    public void CalcuToHour()
    {
        DateTime date1 = new DateTime(2010, 1, 1, 8, 0, 15);
        DateTime date2 = new DateTime(2010, 6, 1, 11, 2, 16);
        TimeSpan interval = date2 - date1;
        Debug.Log(interval);
        DateTime date3 = new DateTime(1,1,1,0,0,0);
        date3 += interval;
        //Debug.Log(date3);
        interval = new TimeSpan(1, 2, 3);
        date2 += interval;
        //Debug.Log(date2);
    }
    //二つのタイムの差を返す関数
    public SEDataTime TimeSpanCalculater(DateTime date1,DateTime date2)
    {
        TimeSpan interval = date1 - date2;
        Debug.Log(interval);
        SEDataTime SEinterval = new SEDataTime(interval); //SEDataTime型に変換
        return SEinterval;
    }
    //SEDataTimeからDateTime型への変換を行う関数
    public DateTime REDataTime(SEDataTime time)
    {
        string stringtime =( time.year+1) + "/" + (time.month+1) + "/" + (time.day+1) + " " + time.hour + ":" + time.minute + ":" + time.second;
        Debug.Log(stringtime);
        DateTime reDataTime = DateTime.Parse(stringtime);
        //DateTime reDataTime = new DateTime(time.year,time.month,time.day,time.hour,time.minute,time.second);
        return reDataTime;
    }
    //次の掃除予定日までの残り時間を返す関数
    public SEDataTime DayDataUntilNextClean(SEDataTime cleanInterval,SEDataTime lastUpdateTime)
    {
        //今日-最終掃除日
        DateTime nowInterval = REDataTime(TimeSpanCalculater(DateTime.Now, REDataTime(lastUpdateTime)));

        //掃除間隔データ-(今日-最終掃除日)
        return TimeSpanCalculater(REDataTime(cleanInterval), nowInterval);
    }

    public List<int> OutDayDatas()
    {
        var result = new List<int>();
        result.Add(year);
        result.Add(month);
        result.Add(day);
        result.Add(hour);
        result.Add(minute);
        result.Add(second);
        return result;
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
