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

    [SerializeField] public SEDataTime lastUpdateTime_forSave;//セーブ用の「最終更新時刻」
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

    //カラーバーに使うやつ=====================================
    //public float floatLastCleanPassTime;
    //public float floatCleanInterval;
    //=====================================

    //ソート用のデータ=====================================
    public int leftDate;
    //=====================================

    public float FloatLastCleanPassTime()
    {
        return LastCleanPassTime.Days;
    }

    public float FloatCleanInterval()
    {
        return CleanInterval.Days;
    }


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

    public void SetLastUpdateTime(DateTime time)
    {
        LastUpdateTime = time;
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

    public void AddPlaceList(CleanPlaceData data)
    {
        placeDataList.Add(data);
        //Debug.Log("=========================data : "+data.LastUpdateTime);
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
