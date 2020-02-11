using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

/// <summary>
/// GameObjectなどを操作し、データを画面に表示するクラス
/// </summary>
public class Main_UI : MainBase,
    IRecieveDayAndNumber,IRecivePushTimeNumber
{
    [SerializeField] GameObject addPlacePanel;//データを追加するときに出てくるパネル
    [SerializeField] InputField addPlaceInputField;//データを追加するときに使うinputField
    [SerializeField] Text displayPlaceText;//プレイスリストのデータを一覧表示するText
    [SerializeField] LayOutTextList layoutTextList;//プレイリストのデータ
    [SerializeField] DisplayCleanPlaceData PlaceDataPanel;//現在選択しているplaceDataの情報を表示するパネル

    [SerializeField] SetIntervalPanel setIntervalPanel;//インターバルの入力をする時のパネル
    [SerializeField] InputField setIntervalDataInputField;//インターバルの入力をするためのinputField
    [SerializeField] Dropdown setIntervalDataDropdownDay;
    [SerializeField] Dropdown setIntervalDataDropdownNumber;
    [SerializeField] GameObject changePanel;//何の変更をするか選択する時のパネル
    [SerializeField] GameObject optinPanel;//通地時間を設定するパネル
    [SerializeField] RenameData RenamePanel;//名前を変更するパネル
    [SerializeField] RemovePanel RemovePanel;//現在選択しているplaceDataの情報を表示するパネル
    [SerializeField] InputField renamePlaceInputField;//名前変更するinputField

    [SerializeField] GameObject homeruPanel;//ほめるパネル

    


    //[SerializeField]int nowTargetIndex=-1;//MainBaseに実装を映したい

    //モードの立ち上がりの処理
    protected override void AwakeModeAction(CurrentMode mode)
    {
        base.AwakeModeAction(mode);
        switch (mode)
        {
            case CurrentMode.DISPLAY:

                NonActiveInputPanel();
                DisplayData();
                break;
            case CurrentMode.ADDPLACEMODE:
                ActiveInputPanel();
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
                setIntervalPanel.gameObject.SetActive(true);
                setIntervalPanel.SetIntervalData(cleanDataList.GetCleanPlaceData(nowTargetIndex));
                setIntervalPanel.IntervalPlaceName();
                break;
            case CurrentMode.CHANGE:
                changePanel.SetActive(true);
                break;
            case CurrentMode.RENAME:
                RenamePanel.gameObject.SetActive(true);
                RenamePanel.SetRenameData(cleanDataList.GetCleanPlaceData(nowTargetIndex));
                RenamePanel.RenameName();
                break;
            case CurrentMode.OPTION:
                optinPanel.SetActive(true);
                break;
            case CurrentMode.HOMERU:
                homeruPanel.SetActive(true);
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
            case CurrentMode.REMOVECHECK:
                RemovePanel.gameObject.SetActive(false);
                break;
            case CurrentMode.REMOVE:
                break;
            case CurrentMode.PLACEDATAMODE:
                PlaceDataPanel.gameObject.SetActive(false);
                break;
            case CurrentMode.SETINTERVALMODE:
                setIntervalPanel.gameObject.SetActive(false);
                setIntervalDataDropdownDay.value = 0;
                setIntervalDataDropdownNumber.value = 0;
                break;
            case CurrentMode.CHANGE:
                changePanel.SetActive(false);
                break;
            case CurrentMode.RENAME:
                RenamePanel.gameObject.SetActive(false);
                InitInputFieldTextRename();
                break;
            case CurrentMode.OPTION:
                optinPanel.SetActive(false);
                break;
            case CurrentMode.HOMERU:
                homeruPanel.SetActive(false);
                break;
        }
    }

    protected override void ChangeModeUpdate(CurrentMode nextMode,bool addOpen)
    {
        base.ChangeModeUpdate(nextMode,addOpen);

        pushCtrl.SetPush_FromCleanPlaceList(cleanDataList.placeDataList);
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
        AddInputData("i");

        Enter();
    }
    //キャンセル押したときに追加ウィンドウを閉じる
    public void ChangeDisplayMode()
    {
        AddInputData("display");
        Enter();
    }
    public void OpenPlaceDataMode(int n)
    {
        if (n < 0) //引数が指定されていない場合（デフォルト値はー1）
        {
            n = nowTargetIndex; //今の開いているplacedataの場所を代入
        }
        SetTargetIndex(n);
        AddInputData("placeData");
        Enter();
    }
    //データの追加と表示
    public void AddPlaceData()
    {
        AddInputData(addPlaceInputField.textComponent.text);
        Enter();
    }
    public void RenamePlaceData()
    {
        AddInputData(renamePlaceInputField.textComponent.text);
        Enter();
    }
    public void ChangeResetMode()
    {
        AddInputData("reset");
        Enter();
    }
    /// <summary>
    /// データの削除
    /// </summary>
    public void RemovePlaceData()
    {
        AddInputData("remove");
        Enter();

        StartCoroutine(WaitFrame(1, () => AddInputData(nowTargetIndex.ToString())));
        StartCoroutine(WaitFrame(1, () => Enter()));
    }


    public void ChengeSetIntervalMode()
    {
        AddInputData("interval");
        Enter();
    }

    public void SetIntervalData()
    {
        AddInputData(setIntervalDataInputField.textComponent.text);
        Enter();
    }
    /// <summary>
    /// changePanelを出す
    /// </summary>
    public void ChangeChangeMode()
    {
        AddInputData("change");
        Enter();
    }
    /// <summary>
    /// removePanelを出す
    /// </summary>
    public void ChangeRemoveCheckMode()
    {
        AddInputData("removecheck");
        Enter();
    }
    public void ChangePlaceDataMode()
    {
        AddInputData("dataUpdateToPlaceData");
        Enter();
    }
    public void ChangeRenameMode()
    {
        AddInputData("rename");
        Enter();
    }

    public void OpenOption()
    {
        AddInputData("option");
        Enter();
    }

    /// <summary>
    /// push通知を送るタイミングを変更する関数
    /// </summary>
    /// <param name="hour"></param>
    public void ChangePushTiming(int hour)
    {
        pushCtrl.SetPushTime(hour);
        cleanDataList.SetPushTIiming(hour);
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
        AddInputData(setIntervalDataDropdownDay.captionText.text);
        Enter();
    }

    public void SetIntervalDataDropdownNumber()
    {
        AddInputData(setIntervalDataDropdownNumber.captionText.text);
        Enter();
    }

    #region 通知関連の関数
    #endregion

    #region Interfaceの関数
    //DayとNumberのデータを受け取るときのインターフェース
    public void RecieveDayAndNumberAction(string Day, int number)
    {
        if (_currentMode == CurrentMode.SETINTERVALMODE)
        {
            AddInputData(Day);
            Enter();
            StartCoroutine(WaitFrame(1, () => AddInputData(number.ToString())));
            StartCoroutine(WaitFrame(1, () => Enter())); 
        }
        else if (_currentMode == CurrentMode.ADDPLACEMODE)
        {

            AddInputData(Day);
            AddInputData(number.ToString());
        }
        
    }

    public void RecivePushTimeNumber(string num)
    {
        AddInputData(num);
        Enter();
    }
    #endregion

    public override void AnimCoal(CurrentMode coalAnimMode)
    {
        base.AnimCoal(coalAnimMode);

        switch (coalAnimMode)
        {
            case CurrentMode.PLACEDATAMODE:
                var anim= PlaceDataPanel.GetComponent<Animator>();
                anim.SetTrigger("Finish");
                break;
            default:
                Debug.Log("not set animation data");
                break;
        }
    }

    #region Debug用の関数
    [ContextMenu("debug_timeToNoon")]
    public void Debug_timeToNoon()
    {
        //SetPush_FromCleanPlaceData(cleanDataList.GetCleanPlaceData(0));
        pushCtrl.SetPush_FromCleanPlaceList(cleanDataList.placeDataList);
    }
    #endregion
}
