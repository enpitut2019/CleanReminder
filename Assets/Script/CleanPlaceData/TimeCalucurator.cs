using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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

    public static DateTime AddTimeAndSpan(DateTime time, TimeSpan span)
    {
        return time + span;
    }


    /// <summary>
    /// 受け取ったDateTimeのhour以下のデータをhour、０、０にする関数
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static DateTime SetDateTimeHour(DateTime time, int hour)
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
