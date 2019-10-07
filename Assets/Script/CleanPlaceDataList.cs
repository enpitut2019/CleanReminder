using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanPlaceDataList : MonoBehaviour
{
    public enum CurrentMode
    {
        DISPLAY,INPUT,ADD
    }
    CurrentMode currentMode = CurrentMode.DISPLAY;

    public List<string> placeList = new List<string>();
    [SerializeField] Text displayText;
    [SerializeField] InputField inputField;
    [SerializeField] GameObject inputPanel;
    string nextAddData;

    private void Update()
    {
        switch (currentMode)
        {
            case CurrentMode.DISPLAY:
                NonActiveInputPanel();
                DisplayData();
                break;
            case CurrentMode.INPUT:
                ActiveInputPanel();
                FocusInputField();
                break;
            case CurrentMode.ADD:
                AddPlaceList();
                SetState(CurrentMode.DISPLAY);
                break;
        }
    }
    #region State関連
    public void SetState(CurrentMode state)
    {
        currentMode = state;
    }

    public void SetStateAdd()
    {
        SetState(CurrentMode.ADD);
    }

    public void SetStateDisplay()
    {
        SetState(CurrentMode.DISPLAY);
    }

    public void SetStateInput()
    {
        SetState(CurrentMode.INPUT);
    }
    #endregion


    public void AddPlaceList()
    {
        if (nextAddData != "")
        {
            placeList.Add(nextAddData);
            ResetInputData();
        }
    }

    public void DisplayData()
    {
        displayText.text = "";
        for(int i=0;i<placeList.Count;i++)
        {
            displayText.text += placeList[i] + "\n";
        }
    }

    public void GetInputData()
    {
        nextAddData = inputField.text;
    }

    public void ResetInputData()
    {
        inputField.text = "";
        nextAddData = "";
    }

    public void ActiveInputPanel()
    {
        inputPanel.SetActive(true);

    }

    public void NonActiveInputPanel()
    {
        inputPanel.SetActive(false);
    }

    public void FocusInputField()
    {
        inputField.ActivateInputField();
    }
}
