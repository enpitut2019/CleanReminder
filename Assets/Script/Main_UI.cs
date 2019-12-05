using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

/// <summary>
/// GameObjectなどを操作し、データを画面に表示するクラス
/// </summary>
public class Main_UI : MainBase,IRecieveDayAndNumber
{
    [SerializeField] GameObject addPlacePanel;//データを追加するときに出てくるパネル
    [SerializeField] InputField addPlaceInputField;//データを追加するときに使うinputField
    [SerializeField] Text displayPlaceText;//プレイスリストのデータを一覧表示するText
    [SerializeField] LayOutTextList layoutTextList;//プレイリストのデータ
    [SerializeField] DisplayCleanPlaceData PlaceDataPanel;//現在選択しているplaceDataの情報を表示するパネル

    [SerializeField] GameObject setIntervalPanel;//インターバルの入力をする時のパネル
    [SerializeField] InputField setIntervalDataInputField;//インターバルの入力をするためのinputField
    [SerializeField] Dropdown setIntervalDataDropdownDay;
    [SerializeField] Dropdown setIntervalDataDropdownNumber;
    [SerializeField] GameObject changePanel;//何の変更をするか選択する時のパネル
    //[SerializeField] GameObject removePanel;//削除の確認するパネル
    [SerializeField] RenameData RenamePanel;//名前を変更するパネル
    [SerializeField] RemovePanel RemovePanel;//現在選択しているplaceDataの情報を表示するパネル
    [SerializeField] InputField renamePlaceInputField;//名前変更するinputField

    [SerializeField] PushObject pushObject;



    //[SerializeField]int nowTargetIndex=-1;//MainBaseに実装を映したい

    //モードの立ち上がりの処理
    protected override void AwakeModeAction(CurrentMode mode)
    {
        base.AwakeModeAction(mode);
        switch (mode)
        {
            case CurrentMode.DISPLAY:

                SetPush_FromCleanPlaceList();
                NonActiveInputPanel();
                DisplayData();
                break;
            case CurrentMode.ADDPLACEMODE:
                ActiveInputPanel();
                break;
            case CurrentMode.DATAUPDATETODISPLAY:
                break;
            case CurrentMode.REMOVECHECK:
                RemovePanel.gameObject.SetActive(true);
                RemovePanel.SetRemoveData(cleanDataList.GetCleanPlaceData(nowTargetIndex));
                RemovePanel.RemoveName();
                break;
            case CurrentMode.PLACEDATAMODE:
                PlaceDataPanel.gameObject.SetActive(true);
                PlaceDataPanel.SetCleanPlaceData(cleanDataList.GetCleanPlaceData(nowTargetIndex));
                PlaceDataPanel.DisplayData();
                break;
            case CurrentMode.SETINTERVALMODE:
                setIntervalPanel.SetActive(true);
                break;
            case CurrentMode.CHANGE:
                changePanel.SetActive(true);
                break;
            case CurrentMode.RENAME:
                RenamePanel.gameObject.SetActive(true);
                RenamePanel.SetRenameData(cleanDataList.GetCleanPlaceData(nowTargetIndex));
                RenamePanel.RenameName();
                break;
        }
    }
    //モード終了時の処理
    protected override void EndModeAction(CurrentMode mode)
    {
        base.EndModeAction(mode);
        switch (mode)
        {
            case CurrentMode.DISPLAY:
                break;
            case CurrentMode.ADDPLACEMODE:
                InitInputFieldText();
                break;
            case CurrentMode.DATAUPDATETODISPLAY:
                break;
            case CurrentMode.REMOVECHECK:
                RemovePanel.gameObject.SetActive(false);
                break;
            case CurrentMode.REMOVE:
                break;
            case CurrentMode.PLACEDATAMODE:
                PlaceDataPanel.gameObject.SetActive(false);
                break;
            case CurrentMode.SETINTERVALMODE:
                setIntervalPanel.SetActive(false);
                setIntervalDataDropdownDay.value = 0;
                setIntervalDataDropdownNumber.value = 0;
                break;
            case CurrentMode.RESET://RESETMODEの時
                break;
            case CurrentMode.CHANGE:
                changePanel.SetActive(false);
                break;
            case CurrentMode.RENAME:
                RenamePanel.gameObject.SetActive(false);
                InitInputFieldTextRename();
                break;
        }
    }
    /// <summary>
    /// 追加ウィンドウの表示
    /// </summary>
    void ActiveInputPanel()
    {
        addPlacePanel.SetActive(true);

    }
    /// <summary>
    ///追加ウィンドウの非表示
    /// </summary>
    void NonActiveInputPanel()
    {
        addPlacePanel.SetActive(false);
    }
    #region ボタンで呼ぶ関数 
    // 入力と入力の確定だけを呼ぶ


    //+ボタンから追加ウィンドウを表示
    public void ChangeAddPlaceMode()
    {
        SetInputData("i");
        Enter();
    }
    //キャンセル押したときに追加ウィンドウを閉じる
    public void ChangeDisplayMode()
    {
        SetInputData("display");
        Enter();
    }
    public void OpenPlaceDataMode(int n)
    {
        if (n < 0) //引数が指定されていない場合（デフォルト値はー1）
        {
            n = nowTargetIndex; //今の開いているplacedataの場所を代入
        }
        SetTargetIndex(n);
        SetInputData("placeData");
        Enter();
    }
    //データの追加と表示
    public void AddPlaceData()
    {
        SetInputData(addPlaceInputField.textComponent.text);
        Enter();
    }
    public void RenamePlaceData()
    {
        SetInputData(renamePlaceInputField.textComponent.text);
        Enter();
    }
    public void ChangeResetMode()
    {
        SetInputData("reset");
        Enter();
    }
    /// <summary>
    /// データの削除
    /// </summary>
    public void RemovePlaceData()
    {
        SetInputData("remove");
        Enter();

        StartCoroutine(WaitFrame(1, () => SetInputData(nowTargetIndex.ToString())));
        StartCoroutine(WaitFrame(1, () => Enter()));
    }


    public void ChengeSetIntervalMode()
    {
        SetInputData("interval");
        Enter();
    }

    public void SetIntervalData()
    {
        SetInputData(setIntervalDataInputField.textComponent.text);
        Enter();
    }
    /// <summary>
    /// changePanelを出す
    /// </summary>
    public void ChangeChangeMode()
    {
        SetInputData("change");
        Enter();
    }
    /// <summary>
    /// removePanelを出す
    /// </summary>
    public void ChangeRemoveCheckMode()
    {
        SetInputData("removecheck");
        Enter();
    }
    public void ChangePlaceDataMode()
    {
        SetInputData("dataUpdateToPlaceData");
        Enter();
    }
    public void ChangeRenameMode()
    {
        SetInputData("rename");
        Enter();
    }

    #region InputDataを扱わないボタン

    #endregion
    #endregion
    /// <summary>
    /// inputFieldの初期化
    /// </summary>
    void InitInputFieldText()
    {
        addPlaceInputField.text = "";
    }

    void InitInputFieldTextRename()
    {
        renamePlaceInputField.text = "";
    }

    /// <summary>
    /// placeListのデータをtextに表示する
    /// </summary>
    void DisplayData()
    {
        layoutTextList.ResetText();
        cleanDataList.DeadLineSort();
        for (int i = 0; i < cleanDataList.placeDataList.Count; i++)
        {
            layoutTextList.AddText(cleanDataList.placeDataList[i]);
            /*layoutTextList.AddText(cleanDataList.GetPlaceData(i)+
                " あと"+cleanDataList.GetCleanPlaceData(i).NextCleanLeftTimeText);*/
        }
    }

    /// <summary>
    /// nフレーム後にuaを実行
    /// </summary>
    /// <param name="n"></param>
    /// <param name="ua"></param>
    /// <returns></returns>
    IEnumerator WaitFrame(int n, UnityAction ua)
    {
        for (int i = 0; i < n; i++)
        {
            yield return null;
        }
        ua.Invoke();
    }

    public void SetIntervalDataDropdownDay()
    {
        SetInputData(setIntervalDataDropdownDay.captionText.text);
        Enter();
    }

    public void SetIntervalDataDropdownNumber()
    {
        SetInputData(setIntervalDataDropdownNumber.captionText.text);
        Enter();
    }

    #region 通知関連の関数
    /// <summary>
    /// cleanPlaceDataの情報をもとに通知を作成する関数
    /// </summary>
    void SetPush_FromCleanPlaceData(CleanPlaceData data)
    {
        DateTime d = TimeCalucurator.SetDateTimeToNoon(data.NextCleanDate);
        if (TimeCalucurator.CheckDate_Today(d)) return;

        pushObject.Push_Scedule(d, 0);
    }
    /// <summary>
    /// cleanPlaceListのデータをもとに通知を作成する関数
    /// これを呼ぶと現在登録されているすべてのデータについて通知が作成される
    /// </summary>
    void SetPush_FromCleanPlaceList()
    {
        var tempList = new List<CleanPlaceData>();//送信する日にちをかぶりなく追加するためのリスト
        foreach(var data in cleanDataList.placeDataList)
        {
            bool addFlag = true;
            foreach(var tdata in tempList)//すでに追加されている日にちかどうかを確認
            {
                if (tdata.NextCleanDate.Day == data.NextCleanDate.Day)//かぶりありならbreak
                {
                    addFlag = false;
                    break;
                }

            }

            if (addFlag)//かぶりなしなら追加
            {
                tempList.Add(data);
            }
        }
        if (tempList == null) return;
        pushObject.SetLastSetDate();
        foreach(var data in tempList)
        {
            SetPush_FromCleanPlaceData(data);
            Debug.Log(data);
        }
    }
    #endregion

    #region Interfaceの関数
    //DayとNumberのデータを受け取るときのインターフェース
    public void RecieveDayAndNumberAction(string Day, int number)
    {

        SetInputData(Day);
        Enter();

        StartCoroutine(WaitFrame(1, () => SetInputData(number.ToString())));
        StartCoroutine(WaitFrame(1, () => Enter())); 
        
    }
    #endregion

    #region Debug用の関数
    [ContextMenu("debug_timeToNoon")]
    public void Debug_timeToNoon()
    {
        //SetPush_FromCleanPlaceData(cleanDataList.GetCleanPlaceData(0));
        SetPush_FromCleanPlaceList();
    }
    #endregion
}


