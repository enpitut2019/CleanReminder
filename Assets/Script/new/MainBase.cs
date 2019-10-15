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
        DISPLAY, ADDPLACEMODE, DATAUPDATE
    }
    [SerializeField]CurrentMode currentMode = CurrentMode.DISPLAY;

    [SerializeField]protected CleanDataListNew cleanDataListNew = new CleanDataListNew();
    [SerializeField] string inputData;//受け取った入力
    [SerializeField] bool inputMode;
 


    // Start is called before the first frame update
    void Start()
    {
        cleanDataListNew.AddPlaceList("unko");
        cleanDataListNew.RemoveData(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (inputMode)
        {
            InputUpdate();
        }
        else
        {
            switch (currentMode)//currentModeごとにUpdate関数を呼び出す
            {
                case CurrentMode.DISPLAY:
                    if (inputData == "i")
                    {
                        ChangeMode(CurrentMode.ADDPLACEMODE);
                        ResetInputData();
                        StartInputMode();
                    }
                    else
                    {
                        StartInputMode();
                    }
                    break;
                case CurrentMode.ADDPLACEMODE:
                    if (inputData == "display")
                    {
                        ChangeMode(CurrentMode.DISPLAY);
                        StartInputMode();
                    }
                    else if (inputData != "")
                    {
                        ChangeMode(CurrentMode.DATAUPDATE);

                    }
                    else
                    {
                        StartInputMode();
                    }
                    break;
                case CurrentMode.DATAUPDATE:
                        cleanDataListNew.AddPlaceList(inputData);
                        ResetInputData();
                        ChangeMode(CurrentMode.DISPLAY);
                    break;
            }
        }
        
    }
    
    void InputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            FinishInputMode();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetInputData("i");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetInputData("display");
        }


    }

    /// <summary>
    /// currentModeの変更
    /// </summary>
    /// <param name="mode"></param>
    void ChangeMode(CurrentMode mode)
    {
        if (currentMode != mode)
        {
            EndModeAction(currentMode);
            AwakeModeAction(mode);
            //ChengeModeActoin(mode);
        }

        currentMode = mode;
        Debug.Log(currentMode);
        
    }

    /*protected virtual void ChengeModeAction(CurrentMode mode)
    {
        switch (mode)
        {
            case CurrentMode.DISPLAY:
                Debug.Log(currentMode);
                break;
            case CurrentMode.ADDPLACEMODE:
                Debug.Log(currentMode);
                break;
        }
    }*/
    protected virtual void EndModeAction(CurrentMode mode)
    {
        switch (mode)
        {
            case CurrentMode.DISPLAY:
                Debug.Log(currentMode);
                break;
            case CurrentMode.ADDPLACEMODE:
                Debug.Log(currentMode);
                break;
        }
    }
    protected virtual void AwakeModeAction(CurrentMode mode)
    {
        switch (mode)
        {
            case CurrentMode.DISPLAY:
                Debug.Log(currentMode);
                break;
            case CurrentMode.ADDPLACEMODE:
                Debug.Log(currentMode);
                break;
        }
    }

    protected virtual bool Inputaa()
    {
        return Input.GetKeyDown(KeyCode.A);
    }

    protected void FinishInputMode()
    {
        inputMode = false;
    }

    void StartInputMode()
    {
        inputMode = true;
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
