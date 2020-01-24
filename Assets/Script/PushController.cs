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
    public bool IsTimeOver { get; private set; }//期日を超えているかどうか


    public PushData(DateTime _pushTime, string _placeName)
    {
        pushTime = _pushTime;
        placeNameList = new List<string>();
        placeNameList.Add(_placeName);
        IsTimeOver = false;
    }
    public PushData(DateTime _pushTime, List<string> _placeName)
    {
        pushTime = _pushTime;
        placeNameList = new List<string>();
        foreach(var data in _placeName)
        {
            placeNameList.Add(data);

        }
        IsTimeOver = false;
    }

    //場所の追加
    public void AddPlace(string _placeName)
    {
        if (!placeNameList.Contains(_placeName))
        {
            placeNameList.Add(_placeName);
        }
    }

    public void SetTimeOver()
    {
        IsTimeOver = true;
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
    [SerializeField] bool isRecievePush = true;//push通知を受け取るかどうか
    public bool IsRecivePush { get { return isRecievePush; } }

    #region 通知
    /// <summary>
    /// PushDataの情報をもとに通知を送る関数
    /// </summary>
    void SetPush_FromPushData(PushData data)
    {
        if (!TimeCalucurator.CheckDate_NotOver(data.pushTime)) return;
//#if UNITY_ANDROID
        if (!data.IsTimeOver)//期限を超えていない場合
        {
            var push= pushObject.Push_scedule(data.pushTime, 0, Create_pushTitle(data), Create_pushMessage(data));
            push.SendPush();
        }
        else
        {
            var push= pushObject.Push_scedule(data.pushTime, 0,
                Create_pushTimeOver()+"。"+Create_pushTitle(data),
                Create_pushTimeOver()+"。"+ Create_pushMessage(data));
            push.SendPush();
        }
//#elif UNITY_IPHONE

//#endif
    }

    /// <summary>
    /// cleanPlaceListのデータをもとに通知を作成する関数
    /// これを呼ぶと現在登録されているすべてのデータについて通知が作成される
    /// </summary>
    public void SetPush_FromCleanPlaceList(List<CleanPlaceData> dataList)
    {
        pushObject.SetLastSetDate();
        if (!isRecievePush) return;

        var tempList = new List<PushData>();//送信する日にちをかぶりなく追加するためのリスト

        #region 掃除期間がオーバーしているものの作成処理
        PushData timeOverPushData = null;//掃除期間がオーバーしているもののデータを作成したかどうか
        foreach (var data in dataList)
        {
            if (data.CheckTimeOver())
            {
                if (timeOverPushData == null)
                {
                    timeOverPushData = new PushData(GetTimeOverTime(), data.Place);
                    timeOverPushData.SetTimeOver();
                }
                else
                {
                    timeOverPushData.AddPlace(data.Place);
                }
            }
        }
        if (timeOverPushData != null)
        {
            for (int i = 0; i < 7; i++)
            {
                tempList.Add(new PushData(timeOverPushData.pushTime.AddDays(i), timeOverPushData.placeNameList));
                tempList[i].SetTimeOver();
            }
        }
        #endregion

        //その他の作成
        foreach (var data in dataList)//送信する日にちごとにPushDataを作成
        {
            DateTime d = TimeCalucurator.SetDateTimeHour(data.NextCleanDate,pushTimingHour);
            bool addFlag = true;
            foreach (var temp in tempList)
            {
                if (temp.pushTime.Equals(d))//既に登録している日にちかどうかのチェック
                {
                    //if (!temp.IsTimeOver) 
                    temp.AddPlace(data.Place);//すでに追加していたら名前のみ追加
                    addFlag = false;
                    break;
                }
            }

            if (addFlag)//かぶりなしなら新規データを作成
            {
                tempList.Add(new PushData(d, data.Place));
            }
        }


        if (tempList == null) return;

        foreach (var data in tempList)
        {
            SetPush_FromPushData(data);
            /*Debug.Log("=================");
            Debug.Log(data.pushTime);
            Debug.Log(data.IsTimeOver);
            Debug.Log("=================");*/
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

    /// <summary>
    /// 掃除期間がオーバーしているときのメッセージを作成
    /// </summary>
    /// <returns></returns>
    string Create_pushTimeOver()
    {
        return "掃除期間をこえているものがあります";
    }
    #endregion
    /// <summary>
    /// 掃除期日を超えているデータの通知時刻を作成
    /// </summary>
    /// <returns></returns>
    DateTime GetTimeOverTime()
    {
        DateTime result = TimeCalucurator.SetDateTimeHour(DateTime.Now, pushTimingHour);
        if (!TimeCalucurator.CheckDate_NotOver(result))
        {
            result = TimeCalucurator.SetDateTimeHour(DateTime.Now.AddDays(1.0), pushTimingHour);
            //Debug.Log(result);
        }
        return result;
    }
}
