using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    [SerializeField] GameObject removePanel;//削除の確認するパネル
    [SerializeField] GameObject renamePanel;//名前を変更するパネル
    [SerializeField] InputField renamePlaceInputField;//名前変更するinputField



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
            case CurrentMode.DATAUPDATETODISPLAY:
                break;
            case CurrentMode.REMOVECHECK:
                removePanel.SetActive(true);

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
                renamePanel.SetActive(true);
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
                removePanel.SetActive(false);
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
                renamePanel.SetActive(false);
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
}


