using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// push内容を保持するデータ
/// </summary>
public class PushData
{
    public List<string> placeNameList { get; private set; }//通知の場所の名前
    public DateTime pushTime { get; private set; }//通知をかける時間


    public PushData(DateTime _pushTime,string _placeName)
    {
        pushTime = _pushTime;
        placeNameList = new List<string>();
        placeNameList.Add(_placeName);
    }

    //場所の追加
    public void AddPlace(string _placeName)
    {
        if (!placeNameList.Contains(_placeName))
        {
            placeNameList.Add(_placeName);
        }
    }
}

/// <summary>
/// push通知の条件を設定するクラス
/// </summary>
public class PushController : MonoBehaviour
{
    [SerializeField] PushObject pushObject;
    [SerializeField] int pushTimingHour;//push通知を送信する時刻
    public int PushTimingHour { get { return pushTimingHour; } }
    #region 通知
    /// <summary>
    /// PushDataの情報をもとに通知を作成する関数
    /// </summary>
    void SetPush_FromPushData(PushData data)
    {
        if (!TimeCalucurator.CheckDate_Over(data.pushTime)) return;
        pushObject.Push_Scedule(data.pushTime, 0,Create_pushMessage(data),Create_pushMessage(data));
    }

    /// <summary>
    /// cleanPlaceListのデータをもとに通知を作成する関数
    /// これを呼ぶと現在登録されているすべてのデータについて通知が作成される
    /// </summary>
    public void SetPush_FromCleanPlaceList(List<CleanPlaceData> dataList)
    {
        var tempList = new List<PushData>();//送信する日にちをかぶりなく追加するためのリスト
        foreach(var data in dataList)
        {
            DateTime d = TimeCalucurator.SetDateTimeHour(data.NextCleanDate,pushTimingHour);
            bool addFlag = true;
            foreach (var temp in tempList)
            {
                if (temp.pushTime.Equals(d))//既に登録している日にちかどうかのチェック
                {
                    temp.AddPlace(data.Place);//すでに追加していたら名前のみ追加
                    addFlag = false;
                    break;
                }
            }

            if (addFlag)//かぶりなしなら追加
            {
                tempList.Add(new PushData(d, data.Place));
            }
        }

        if (tempList == null) return;
        pushObject.SetLastSetDate();
        foreach (var data in tempList)
        {
            SetPush_FromPushData(data);
            Debug.Log(data.pushTime);
        }
    }
    #endregion
    /// <summary>
    /// 何時に通知が来るかを設定する関数
    /// </summary>
    /// <param name="hour"></param>
    public void SetPushTiming(int hour)
    {
        pushTimingHour = hour;
    }
    #region messageの作成

    /// <summary>
    /// push通知のメッセージを作成する関数
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    string Create_pushMessage(PushData data)
    {
        string message = "今日は";
        int count = 0;
        foreach (var d in data.placeNameList)
        {
            if (count != 0) message += "、";
            message += d;
            count++;
        }
        message += "を掃除する日です";
        return message;
    }

    /// <summary>
    /// psuh通知のタイトルを作成する関数
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    string Create_pushTitle(PushData data)
    {

        string title = "今日は";
        if (data.placeNameList.Count == 1)
        {
            title += data.placeNameList[0] + "を掃除する日です";
        }
        else
        {
            title += data.placeNameList[0] + "など" + data.placeNameList.Count + "箇所を" + "を掃除する日です";
        }

        return title;
    }
    #endregion
}
