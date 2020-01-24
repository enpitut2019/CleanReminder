using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public static string GetLastDateTime(DateTime time)
    {
        return time.Year + "年" + time.Month + "月" + time.Day + "日";
    }

}