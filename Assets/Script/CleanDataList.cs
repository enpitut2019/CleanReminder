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
    public string Place { get { return place; } private set { place = value; } }

    [SerializeField] SEDataTime lastUpdateTime_forSave;//セーブ用の「最終更新時刻」
    [SerializeField] SEDataTime cleanInterval_forSave;//セーブ用の「掃除間隔データ」

    //時間などの文字列データ=============================================
    public string LastUpdateTimeText { get { return TimeCovertToString.GetDateTime(LastUpdateTime); } }
    public string CleanIntervalText { get { return TimeCovertToString.GetTimeSpan(CleanInterval); } }
    public string NextCleanLeftTimeText { get { return TimeCovertToString.GetTimeSpan(NextCleanLeftTime); } }
    //==============================================

    //日時計算用のデータ============================
    public DateTime LastUpdateTime { get; private set; }//最終更新時刻
    public TimeSpan CleanInterval { get; protected set; }//掃除間隔
    public DateTime NextCleanDate { get { return LastUpdateTime + CleanInterval; } }//次に掃除する日時
    public TimeSpan LastCleanPassTime { get { return DateTime.Now - LastUpdateTime; } }//最後に掃除してからの経過時間
    public TimeSpan NextCleanLeftTime { get { return NextCleanDate - DateTime.Now; } }//次に掃除するまでの時間
    //=============================

    //ソート用のデータ=====================================
    public int leftDate;
    //=====================================

    public int GetLeftDay()
    {
        return leftDate = NextCleanLeftTime.Days;
    }
    
    public CleanPlaceData(string place)
    {
        this.place = place;
        LastUpdateTime = DateTime.Now;
        CleanInterval = new TimeSpan();
        SetSETime();
    }
    #region SEDataTimeへの操作
    public bool CheckHaveTarget()
    {
        return cleanInterval_forSave.CheackHaveTarget();
    }

    public bool SetTarget(string key)
    {
        return cleanInterval_forSave.ChangeTarget(key);
    }

    public void SetCleanIntervalDate(int i)
    {
        cleanInterval_forSave.ChangeDate(i);//SEのほうを変更
        SetDateTime();//TimeSpanをSEに同期

    }
    #endregion

    public void ResetLastUpdateTime()//lastUpdateTimeを現在の時間にする.
    {
        LastUpdateTime = DateTime.Now;
        SetSETime();
    }
    /// <summary>
    /// DateTime->SEへの変換
    /// </summary>
    void SetSETime()
    {
        lastUpdateTime_forSave = new SEDataTime(LastUpdateTime);
        cleanInterval_forSave = new SEDataTime(CleanInterval);
    }

    /// <summary>
    /// SE->DateTImeへの変換
    /// </summary>
    void SetDateTime()
    {
        LastUpdateTime = TimeCalucurator.REDataTime(lastUpdateTime_forSave);
        CleanInterval = TimeCalucurator.ReTimeSpan(cleanInterval_forSave);
    }


    public void ChangeCleanTimeSpan(int num)
    {
        cleanInterval_forSave.ChangeDate(num);
        SetDateTime();
    }

    public void InitAction()
    {
        SetDateTime();
    }

    public void SetPlaceName(String name)
    {
        Place = name;
    }
    #region bool関数
    /// <summary>
    /// 掃除期間がオーバーしているかどうかを取得
    /// </summary>
    public bool CheckTimeOver()
    {
        return NextCleanLeftTime.Days < 0;
    }
    #endregion
}

/// <summary>
/// 時間データをstringで返す関数群
/// </summary>
public class TimeCovertToString
{
    public static string GetTimeSpan(TimeSpan time)
    {
        string result = "";
        result += time.Days + "日";
        return result;
    }

    public static string GetDateTime(DateTime time)
    {
        return time.Year + "年" + time.Month + "月" + time.Day + "日" + time.Hour + "時" + time.Minute + "分" + time.Second + "秒";
    }
}

/// <summary>
/// 時間の計算をする関数群
/// </summary>
public class TimeCalucurator
{
    public static TimeSpan GetTimeSpan(DateTime frontTime, DateTime backTime)
    {
        return frontTime - backTime;
    }
    public static DateTime REDataTime(SEDataTime time)
    {
        var datas = time.OutDayDatas();
        DateTime reDataTime = new DateTime(datas[0], datas[1], datas[2], datas[3], datas[4], datas[5]);
        return reDataTime;
    }

    public static TimeSpan ReTimeSpan(SEDataTime time)
    {
        var datas = time.OutDayDatas();
        TimeSpan reDataTime = new TimeSpan(datas[0] * 365 + datas[1] * 30 + datas[2], datas[3], datas[4], datas[5]);
        return reDataTime;
    }

    public static DateTime AddTimeAndSpan(DateTime time,TimeSpan span)
    {
        return time + span;
    }


    /// <summary>
    /// 受け取ったDateTimeのhour以下のデータをhour、０、０にする関数
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static DateTime SetDateTimeHour(DateTime time,int hour)
    {
        return new DateTime(time.Year, time.Month, time.Day, hour, 0, 0);
    }

    /// <summary>
    /// 受け取ったDateTimeが今日なのかを返す
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static bool CheckDate_Today(DateTime time)
    {
        return time.Day == DateTime.Now.Day;
    }
    /// <summary>
    /// 受け取ったDateTimeが未来なのかを返す
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static bool CheckDate_NotOver(DateTime time)
    {
        return DateTime.Now < time;
    }

}

[System.Serializable]
public class SEDataTime
{
    [SerializeField] int year;
    [SerializeField] int month;
    [SerializeField] int day;
    [SerializeField] int hour;
    [SerializeField] int minute;
    [SerializeField] int second;
    string targetkey = "";
    Dictionary<string, int> dataTimeDictionary;



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
        //var day_temp = time.Days;
        this.year = 0;
        this.month = 0;
        /*while (day_temp > 365)
        {
            day_temp -= 365;
            this.year += 1;
        }

        while (day_temp > 30)
        {
            day_temp -= 30;
            this.month += 1;
        }*/

        this.day = time.Days;
        this.hour = time.Hours;
        this.minute = time.Minutes;
        this.second = time.Seconds;
    }

    public  SEDataTime(string Day, int number)
    {
        ChangeTarget(Day);
        ChangeDate(number);
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
        targetkey = "";
        return false;
    }

    public bool CheackHaveTarget()
    {
        return !(targetkey == "");
    }

    public void ChangeDate(int value)
    {
        if (dataTimeDictionary == null)
        {
            InitDictionary();
        }
        if (dataTimeDictionary.ContainsKey(targetkey))
        {
            year = 0;
            month = 0;
            day = 0;
            hour = 0;
            minute = 0;
            second = 0;

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

    public void ChangeDate(string key, int value)
    {
        ChangeTarget(key);
        ChangeDate(value);
    }

    public int GetDate(string key)
    {
        if (dataTimeDictionary == null)
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
        DateTime date3 = new DateTime(1, 1, 1, 0, 0, 0);
        date3 += interval;
        //Debug.Log(date3);
        interval = new TimeSpan(1, 2, 3);
        date2 += interval;
        //Debug.Log(date2);
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
    [SerializeField] int pushTiming=18;//push通知を送るタイミングのセーブデータ
    public int PushTiming { get { return pushTiming; } }

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

    public void RenamePlaceList(string placename, int index)
    {
        //placeDataList[index].Place = placename;                   aaaaaaa
        CleanPlaceData data = placeDataList[index];
        data.SetPlaceName(placename);
        //placeDataList[index].SetPlaceName(placename);
    }

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
        return placeDataList[index].LastUpdateTimeText;
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

    public List<CleanPlaceData> DeadLineSort()
    {
        placeDataList.Sort((a,b) => a.GetLeftDay() - b.GetLeftDay());
        return placeDataList;
    }

    /// <summary>
    /// プッシュタイミングの変更
    /// </summary>
    /// <param name="time"></param>
    public void SetPushTIiming(int time)
    {
        pushTiming = time;
    }
}
