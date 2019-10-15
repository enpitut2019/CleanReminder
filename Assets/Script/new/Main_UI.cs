using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Main_UI : MainBase
{
    [SerializeField] GameObject inputPanel;
    [SerializeField] InputField inputField;
    [SerializeField] Text displayText;



    /*protected override void ChengeModeAction(CurrentMode mode)
    {
        switch (mode)
        {
            case CurrentMode.DISPLAY:
                NonActiveInputPanel();
                break;
            case CurrentMode.ADDPLACEMODE:
                ActiveInputPanel();
                break;
        }
    }*/
    //モードの立ち上がりの処理
    protected override void AwakeModeAction(CurrentMode mode)
    {
        base.AwakeModeAction(mode);
        switch (mode)
        {
            case CurrentMode.DISPLAY:
                NonActiveInputPanel();
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
                SetInputFieldText();
                break;
            case CurrentMode.DATAUPDATE:
                DisplayData();
                break;
        }
    }
    //追加ウィンドウの表示
    public void ActiveInputPanel()
    {
        inputPanel.SetActive(true);

    }
    //追加ウィンドウの非表示
    public void NonActiveInputPanel()
    {
        inputPanel.SetActive(false);
    }
    //+ボタンから追加ウィンドウを表示
    public void ChangeAddPlaceMode()
    {
        SetInputData("i");
        FinishInputMode();
    }
    //キャンセル押したときに追加ウィンドウを閉じる
    public void ChangeInputMode()
    {
        SetInputData("display");
        FinishInputMode();
    }
    //データの追加と表示
    public void AddPlaceData()
    {
        SetInputData(inputField.textComponent.text);
        FinishInputMode();
        SetInputFieldText();
        DisplayData();
    }
    //inputFieldの初期化
    void SetInputFieldText()
    {
        inputField.text = "";
    }
    //配列中身の表示
    void DisplayData()
    {
        displayText.text = "";
        for (int i = 0; i < cleanDataListNew.placeList.Count; i++)
        {
            displayText.text += cleanDataListNew.placeList[i] + "\n";
        }
    }

}
