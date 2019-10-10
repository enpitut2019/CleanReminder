using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : MonoBehaviour
{
    /// <summary>
    /// 現在の状態
    /// </summary>
    public enum CurrentMode
    {
        DISPLAY, INPUT, ADD
    }
    [SerializeField]CurrentMode currentMode = CurrentMode.DISPLAY;

    [SerializeField]CleanDataListNew cleanDataListNew = new CleanDataListNew();
    [SerializeField] string inputData;//受け取った入力

    
    // Start is called before the first frame update
    void Start()
    {
        cleanDataListNew.AddPlaceList("unko");
        cleanDataListNew.RemoveData(0);

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentMode)//currentModeごとにUpdate関数を呼び出す
        {
            case CurrentMode.DISPLAY:
                CurrentUpdate_Display();
                break;
            case CurrentMode.INPUT:
                CurrentUpdate_Input();
                break;
            case CurrentMode.ADD:
                CurrentUpdate_Add();
                break;
        }
    }
    
    /// <summary>
    /// currentModeの変更
    /// </summary>
    /// <param name="mode"></param>
    void ChangeMode(CurrentMode mode)
    {
        currentMode = mode;
        Debug.Log(currentMode);
    }
    #region CurrentUpdate
    protected virtual void CurrentUpdate_Display()
    {
        if (Inputaa())
        {
            ChangeMode(CurrentMode.INPUT);
        }
    }

    protected virtual void CurrentUpdate_Input()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetInputData(inputData + "b");
        }
        if (Inputaa())
        {
            ChangeMode(CurrentMode.ADD);
        }
    }

    protected virtual void CurrentUpdate_Add()
    {
        cleanDataListNew.AddPlaceList(inputData);
        ChangeMode(CurrentMode.DISPLAY);
        ResetInputData();
    }
    #endregion
    protected virtual bool Inputaa()
    {
        return Input.GetKeyDown(KeyCode.A);
    }

    /// <summary>
    /// inputDataの変更
    /// </summary>
    /// <param name="data"></param>
    protected virtual void SetInputData(string data)
    {
        inputData = data;
    }

    /// <summary>
    /// inputDataの初期化
    /// </summary>
    protected virtual void ResetInputData()
    {
        inputData = "";
    }
}
