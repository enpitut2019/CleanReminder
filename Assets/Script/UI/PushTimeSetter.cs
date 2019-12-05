using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushTimeSetter : MonoBehaviour
{
    PushController pushController;
    [SerializeField] Dropdown myDropDown;
    [SerializeField] Main_UI mainUI;
    [SerializeField] Text nowTimeText;//現在の通知時刻の内容

    private void Start()
    {
    }
    private void OnEnable()
    {
        pushController = FindObjectOfType<PushController>();
        DataUpdate();
    }

    public void ButtonAction_SetHour()
    {
        mainUI.ChangePushTiming(int.Parse( myDropDown.captionText.text));
        DataUpdate();
    }

    void DisplayNowTiming()
    {
        nowTimeText.text = "現在の設定時刻" + pushController.PushTimingHour + "時";
    }

    void DataUpdate()
    {
        DisplayNowTiming();
        myDropDown.value = 0;
    }
}
