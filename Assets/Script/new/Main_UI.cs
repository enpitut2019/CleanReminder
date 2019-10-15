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

    protected override void EndModeAction(CurrentMode mode)
    {
        base.EndModeAction(mode);
        switch (mode)
        {
            case CurrentMode.DISPLAY:
                break;
            case CurrentMode.ADDPLACEMODE:
                break;
            case CurrentMode.DATAUPDATE:
                DisplayData();
                break;
        }
    }

    public void ActiveInputPanel()
    {
        inputPanel.SetActive(true);

    }
    public void NonActiveInputPanel()
    {
        inputPanel.SetActive(false);
    }
    public void ChangeAddPlaceMode()
    {
        SetInputData("i");
        FinishInputMode();
    }
    public void ChangeInputMode()
    {
        SetInputData("display");
        FinishInputMode();
    }
    public void AddPlaceData()
    {
        SetInputData(inputField.text);
        FinishInputMode();
        SetInputFieldText();
        DisplayData();
    }
    void SetInputFieldText()
    {
        inputField.text = "";
    }
    void DisplayData()
    {
        displayText.text = "";
        for (int i = 0; i < cleanDataListNew.placeList.Count; i++)
        {
            displayText.text += cleanDataListNew.placeList[i] + "\n";
        }
    }

}
