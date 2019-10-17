using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameObjectなどを操作し、データを画面に表示するクラス
/// </summary>
public class Main_UI : MainBase
{
    [SerializeField] GameObject addPlacePanel;//データを追加するときに出てくるパネル
    [SerializeField] InputField addPlaceInputField;//データを追加するときに使うinputField
    [SerializeField] Text displayPlaceText;//プレイスリストのデータを一覧表示するText

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
            case CurrentMode.DATAUPDATE:
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
            case CurrentMode.DATAUPDATE:
                //DisplayData();
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
    public void ChangeInputMode()
    {
        SetInputData("display");
        Enter();
    }
    //データの追加と表示
    public void AddPlaceData()
    {
        SetInputData(addPlaceInputField.textComponent.text);
        Enter();
    }
    #endregion
    /// <summary>
    /// inputFieldの初期化
    /// </summary>
    void InitInputFieldText()
    {
        addPlaceInputField.text = "";
    }

    /// <summary>
    /// placeListのデータをtextに表示する
    /// </summary>
    void DisplayData()
    {
        displayPlaceText.text = "";
        for (int i = 0; i < cleanDataList.placeList.Count; i++)
        {
            displayPlaceText.text += cleanDataList.placeList[i] + "\n";
        }
    }

}
