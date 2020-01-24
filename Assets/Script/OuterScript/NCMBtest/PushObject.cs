using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using NCMB;
using System;

public class PushObject : MonoBehaviour,IRecieveDayAndNumber
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

    [SerializeField] Text objectIdDebug;//画面でオブジェクトIDを表示するためのText

    string lastSetDate;

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

    public void Push_scedule(float offset, string _title = null, string _message = null)
    {
        Push_scedule(System.DateTime.Now, offset,_title,_message);
    }
    //Push通知の内容を作成する関数
    public NCMBPush Push_scedule(DateTime fromTime, float offset,string _title=null,string _message=null)
    {
        NCMBPush push = new NCMBPush();
        push.Title = (_title==null)? "掃除の日です！！":_title;
        push.Message = (_message==null)?sendPushMessage:_message;
        push.DeliveryTime =fromTime.AddSeconds(offset);

        //Debug.Log("push Sceduled Time is "+push.DeliveryTime);
        //Debug.Log(_title);
        //Debug.Log(_message);
        //絞り込み処理 ObjectIDの設定=============================

        var key = GetObjectId();
        DisplayObjectId(key);
        push.SearchCondition = new Dictionary<string, string>()
        {
            {"objectId",key },
            {"lastSetDate",lastSetDate}
        };
        //=============================
        //push.Dialog = true;
        //配信OSの設定===========================
#if UNITY_ANDROID
        push.PushToAndroid = true;
#elif UNITY_IPHONE
        push.PushToIOS = true;
#endif
        //push.SendPush();
        return push;
    }

    public void RecieveDayAndNumberAction(string Day, int number)
    {
        var time = new SEDataTime(Day, number);
        DateTime targetDay = TimeCalucurator.AddTimeAndSpan(DateTime.Now, TimeCalucurator.ReTimeSpan(time));
        Push_scedule(targetDay,0);
    }

    /// <summary>
    /// 最終更新時刻の更新（pushを受け取るキー）
    /// </summary>
    public void SetLastSetDate()
    {
        lastSetDate = DateTime.Now.ToString();

        NCMBInstallation inst = NCMBInstallation.getCurrentInstallation();
        inst.Remove("lastSetDate");
        inst.Add("lastSetDate",lastSetDate);
        inst.SaveAsync((NCMBException e) => {
            if (e != null)
            {
                //成功時の処理
            }
            else
            {
                //エラー時の処理
            }
        });
    }


    //ObjectIdを画面に表示する関数
    void DisplayObjectId(string key)
    {
        objectIdDebug.text = key;
        Debug.Log(key);
    }

    string GetObjectId()
    {
        NCMBInstallation inst = NCMBInstallation.getCurrentInstallation();
        return inst.ObjectId;
    }
}
