using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 機能のみを記述する
/// GameObjectを参照してはいけない
/// </summary>
public class MainBase : MonoBehaviour
{
    /// <summary>
    /// 現在の状態
    /// </summary>
    public enum CurrentMode
    {
        START,//最初に選択されている状態
        DISPLAY,//掃除場所の一覧を表示している状態
        ADDPLACEMODE,//場所のデータを追加する状態
        DATAUPDATE,//データを更新する状態
        REMOVE,//データを削除する状態
        PLACEDATAMODE//場所のデータの詳細を表示している状態
    }
    [SerializeField]CurrentMode currentMode = CurrentMode.DISPLAY;
    [SerializeField]protected CleanDataList cleanDataList = new CleanDataList();//掃除場所のデータリストを扱うクラス
    [SerializeField] string inputData;//受け取った入力を入れる変数
    [SerializeField] bool canInput;//入力受け取り状態を表す変数
    
    DataSaveClass dataSave = new DataSaveClass();//セーブとロードを行うクラス
    #region データをセーブするpath群
    string cleanDataListPath = "cleanPlaceData";
    #endregion
    void Start()
    {
        LoadData();
        ChangeMode(CurrentMode.DISPLAY);
    }
    
    void Update()
    {
        if (canInput)
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
                        WaitInput();
                    }else if (inputData =="placeData")
                    {
                        ChangeMode(CurrentMode.PLACEDATAMODE);
                        ResetInputData();
                        WaitInput();
                    }
                    else
                    {
                        WaitInput();
                    }
                    break;
                case CurrentMode.ADDPLACEMODE:
                    if (inputData == "display")
                    {
                        ChangeMode(CurrentMode.DISPLAY);
                        WaitInput();
                    }
                    else if (inputData != "")
                    {
                        ChangeMode(CurrentMode.DATAUPDATE);
                        cleanDataList.AddPlaceList(inputData);
                    }
                    else
                    {
                        WaitInput();
                    }
                    break;
                case CurrentMode.DATAUPDATE:
                        ResetInputData();
                        ChangeMode(CurrentMode.DISPLAY);
                        SaveData();
                    break;
                case CurrentMode.REMOVE:
                    int num = 0;
                    bool result = int.TryParse(inputData, out num);
                    if (result)//入力が数字だった時
                    {
                        cleanDataList.RemoveData(num);
                        ChangeMode(CurrentMode.DATAUPDATE);
                        ResetInputData();
                    }
                    else//入力が数字以外だった場合
                    {
                        ResetInputData();
                        WaitInput();
                    }
                    
                    break;
                case CurrentMode.PLACEDATAMODE:
                    if (inputData == "display")
                    {
                        ChangeMode(CurrentMode.DISPLAY);
                        ResetInputData();
                    }
                    else if(inputData == "remove")
                    {
                        ChangeMode(CurrentMode.REMOVE);
                        ResetInputData();
                        WaitInput();
                    }
                    break;
            }
        }
        
    }

    /// <summary>
    /// キー入力でデータを入力するための関数
    /// デバック用
    /// </summary>
    void InputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Enter();//入力の確定
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetInputData("i");//AddPlaceModeに入るための文字列の入力
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetInputData("display");//DisPlayModeに入るための文字列の入力
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetInputData("remove");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetInputData("placeData");
        }
    }

    /// <summary>
    /// currentModeの変更
    /// </summary>
    /// <param name="targetMode"></param>
    void ChangeMode(CurrentMode targetMode)
    {
        if (currentMode == targetMode)
        {
            Debug.Log("おなじモードが呼び出されています");
            return;
        }
        EndModeAction(currentMode);
        AwakeModeAction(targetMode);
        currentMode = targetMode;

    }
    /// <summary>
    /// モードが終了したときの処理
    /// </summary>
    /// <param name="mode"></param>
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
    /// <summary>
    /// モードが起動したときの処理
    /// </summary>
    /// <param name="mode"></param>
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
    
    /// <summary>
    /// 入力の確定
    /// </summary>
    protected void Enter()
    {
        canInput = false;
    }

    /// <summary>
    /// 入力待ちの開始
    /// </summary>
    void WaitInput()
    {
        canInput = true;
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
    /// <summary>
    /// データをjsonファイルに書き込む
    /// </summary>
    void SaveData(){
        dataSave.SaveData<CleanDataList>(cleanDataList, cleanDataListPath);
    }

    /// <summary>
    /// データをjsonファイルから読み込み
    /// </summary>
    void LoadData(){
        cleanDataList=dataSave.LoadData<CleanDataList>(cleanDataListPath);
    }
}
