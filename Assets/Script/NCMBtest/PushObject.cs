using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NCMB;
using System;

public class PushObject : MonoBehaviour,RecieveDayAndNumber
{
    private static bool _isInitialized = false;

    /// <summary>
    ///イベントリスナーの登録
    /// </summary>
    void OnEnable()
    {
        NCMBManager.onRegistration += OnRegistration;
        NCMBManager.onNotificationReceived += OnNotificationReceived;
    }

    /// <summary>
    ///イベントリスナーの削除
    /// </summary>
    void OnDisable()
    {
        NCMBManager.onRegistration -= OnRegistration;
        NCMBManager.onNotificationReceived -= OnNotificationReceived;
    }

    /// <summary>
    ///端末登録後のイベント
    /// </summary>
    void OnRegistration(string errorMessage)
    {
        if (errorMessage == null)
        {
            Debug.Log("OnRegistrationSucceeded");
        }
        else
        {
            Debug.Log("OnRegistrationFailed:" + errorMessage);
        }
    }

    /// <summary>
    ///メッセージ受信後のイベント
    /// </summary>
    void OnNotificationReceived(NCMBPushPayload payload)
    {
        Debug.Log("OnNotificationReceived");
    }

    /// <summary>
    ///シーンを跨いでGameObjectを利用する設定
    /// </summary>
    public virtual void Awake()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    /////////////////////////ここまで追加コード////////////////////////

    [SerializeField] float sendPushWaitTime;
    [SerializeField] string sendPushMessage;

    // Use this for initialization
    void Start()
    {
        /*NCMBPush push = new NCMBPush();
        push.Title = "Notification";
        push.Message = "testSendPush";
        push.ImmediateDeliveryFlag = true;
        push.Dialog = true;
        push.PushToAndroid = true;
        push.SendPush();*/

        //Scedule(sendPushWaitTime);
    }

    public void Scedule(float offset)
    {
        Scedule(System.DateTime.Now, offset);
    }
    public void Scedule(System.DateTime fromTime, float offset)
    {
        NCMBPush push = new NCMBPush();
        push.Title = "Test";
        push.Message = sendPushMessage;
        push.DeliveryTime =fromTime.AddSeconds(offset);
        //push.Dialog = true;
        push.PushToAndroid = true;
        push.SendPush();
    }

    public void RecieveDayAndNumberAction(string Day, int number)
    {
        var time = new SEDataTime(Day, number);
        DateTime targetDay = TimeCalucurator.AddTimeAndSpan(DateTime.Now, TimeCalucurator.ReTimeSpan(time));
        Scedule(targetDay,0);
    }

}
