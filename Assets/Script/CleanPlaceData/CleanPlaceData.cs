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
    public string LastCleanDayTimeText { get { return TimeCovertToString.GetLastDateTime(LastUpdateTime); } }
    public string CleanIntervalText { get { return TimeCovertToString.GetTimeSpan(CleanInterval); } }
    public string NextCleanLeftTimeText { get { return TimeCovertToString.GetTimeSpan(NextCleanLeftTime); } }
    //==============================================

    //日時計算用のデータ==日付データを０時に統一==========================
    public DateTime LastUpdateTime { get; private set; }//最終更新時刻
    public TimeSpan CleanInterval { get; protected set; }//掃除間隔
    public DateTime NextCleanDate { get { return LastUpdateTime + CleanInterval; } }//次に掃除する日時
    public TimeSpan LastCleanPassTime { get { return TimeCalucurator.SetDateTimeHour(DateTime.Now, 0) - LastUpdateTime; } }//最後に掃除してからの経過時間
    public TimeSpan NextCleanLeftTime { get { return NextCleanDate - TimeCalucurator.SetDateTimeHour(DateTime.Now, 0); } }//次に掃除するまでの時間
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
        //LastUpdateTime = DateTime.Now;
        ResetLastUpdateTime();
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

        //LastUpdateTime = DateTime.Now;
        LastUpdateTime = TimeCalucurator.SetDateTimeHour(DateTime.Now, 0);
        SetSETime();
    }

    public void SetLastUpdateTime(DateTime time)
    {
        //LastUpdateTime = time;
        LastUpdateTime = TimeCalucurator.SetDateTimeHour(time, 0);
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