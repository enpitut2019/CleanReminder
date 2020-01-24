using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushTimeSetter : MonoBehaviour
{
    PushController pushController;
    [SerializeField] Dropdown myDropDown;
    [SerializeField] Text nowTimeText;//現在の通知時刻の内容

    [SerializeField] GameObject target;//データを送る対象　IRecievePushTimeNumberを実装してなければいけない
    IRecivePushTimeNumber targetInterface;

    private void Start()
    {
        targetInterface = target.GetComponent<IRecivePushTimeNumber>();
        if (targetInterface == null)
        {
            Debug.Log("PushTimeSetter : target is null or but");
        }
    }
    private void OnEnable()
    {
        pushController = FindObjectOfType<PushController>();
        StartCoroutine( DataUpdate(3));
    }

    public void ButtonAction_SetHour()
    {
        //mainUI.ChangePushTiming(int.Parse( myDropDown.captionText.text));
        targetInterface.RecivePushTimeNumber(myDropDown.captionText.text);
        StartCoroutine( DataUpdate(3));
    }

    void DisplayNowTiming()
    {
        nowTimeText.text = "現在の設定時刻" + pushController.PushTimingHour + "時";
    }

    IEnumerator DataUpdate(int frame)
    {
        for(int i = 0; i < frame;i++)
        {
            yield return null;
        }
        DisplayNowTiming();
        myDropDown.value = pushController.PushTimingHour;
    }

}
