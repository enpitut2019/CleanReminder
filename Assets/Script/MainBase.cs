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
        DATAUPDATETODISPLAY,//データを更新してDISPLAYに戻る状態状態
        DATAUPDATETOPLACEDATA,//データを更新してPLACEDATAに戻る状態状態
        REMOVECHECK,//削除するか確認する状態
        REMOVE,//データを削除する状態
        PLACEDATAMODE,//場所のデータの詳細を表示している状態
        SETINTERVALMODE,//掃除する間隔の登録をする状態
        RESET,//最終掃除時間のリセットができる状態
        CHANGE,//変更ボタンを押して何を変更するか選択する状態
        RENAME//名前を変更する状態
    }
    [SerializeField]CurrentMode currentMode = CurrentMode.DISPLAY;
    [SerializeField]protected CleanDataList cleanDataList = new CleanDataList();//掃除場所のデータリストを扱うクラス
    [SerializeField] string inputData;//受け取った入力を入れる変数
    [SerializeField] bool canInput;//入力受け取り状態を表す変数

    [SerializeField] protected int nowTargetIndex = -1;//MainBaseに実装を映したい

    DataSaveClass dataSave = new DataSaveClass();//セーブとロードを行うクラス
    #region データをセーブするpath群
    string cleanDataListPath = "cleanPlaceData";
    #endregion
    void Start()
    {
        LoadData();
        foreach(var d in cleanDataList.placeDataList)
        {
            d.InitAction();
        }

        ChangeMode(CurrentMode.DISPLAY);

        
        SEDataTime data = new SEDataTime();

        //data.CalcuToHour();
        //data.TimeSpanCalculater();
        //data.REDataTime();
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
                    }
                    else if (inputData == "placeData")
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
                        ChangeMode(CurrentMode.DATAUPDATETODISPLAY);
                        cleanDataList.AddPlaceList(inputData);
                    }
                    else
                    {
                        WaitInput();
                    }
                    break;
                case CurrentMode.DATAUPDATETODISPLAY:
                    ResetInputData();
                    ChangeMode(CurrentMode.DISPLAY);
                    SaveData();
                    break;
                case CurrentMode.DATAUPDATETOPLACEDATA:
                    ResetInputData();
                    ChangeMode(CurrentMode.PLACEDATAMODE);
                    SaveData();
                    break;
                case CurrentMode.REMOVECHECK:
                    {
                        if (inputData == "remove")//削除ボタンを押した時
                        {
                            ChangeMode(CurrentMode.REMOVE);
                            ResetInputData();
                        }
                        else if (inputData == "placeData")//キャンセルボタンを押した時
                        {
                            ChangeMode(CurrentMode.PLACEDATAMODE);
                            ResetInputData();
                        }
                        break;
                    }
                case CurrentMode.REMOVE:
                    {
                        int num = 0;
                        bool result = int.TryParse(inputData, out num);
                        if (result)//入力が数字だった時
                        {
                            cleanDataList.RemoveData(num);
                            ChangeMode(CurrentMode.DATAUPDATETODISPLAY);
                            //ResetInputData();
                        }
                        else//入力が数字以外だった場合
                        {
                            ResetInputData();
                            WaitInput();
                        }

                        break;
                    }
                case CurrentMode.PLACEDATAMODE:
                    if (inputData == "test")
                    {
                        var nowData = cleanDataList.GetCleanPlaceData(nowTargetIndex);
                        /*Debug.Log("Next limit : "+nowData.NextCleanDate);
                        Debug.Log("Next span : " + nowData.NextCleanLeftTime.Days);
                        Debug.Log("Next span : " + nowData.NextCleanLeftTime.Hours);
                        Debug.Log("Next span : " + nowData.NextCleanLeftTime.Minutes);*/
                        Debug.Log(nowData.NextCleanLeftTimeText);


                        WaitInput();
                    }

                    if (inputData == "display")
                    {
                        ChangeMode(CurrentMode.DISPLAY);
                        ResetInputData();
                    }
                    else if (inputData == "interval")
                    {
                        ChangeMode(CurrentMode.SETINTERVALMODE);
                        ResetInputData();
                        WaitInput();
                    }
                    else if (inputData == "reset")
                    {
                        ChangeMode(CurrentMode.RESET);
                        ResetInputData();
                    }
                    else if (inputData == "change")
                    {
                        ChangeMode(CurrentMode.CHANGE);
                        ResetInputData();
                        WaitInput();
                    }
                    break;
                case CurrentMode.SETINTERVALMODE:
                    {
                        int num = 0;
                        Debug.Log(inputData);
                        bool result = int.TryParse(inputData, out num);
                        if (inputData == "display")
                        {
                            ChangeMode(CurrentMode.DISPLAY);
                            ResetInputData();
                        }
                        if (result)//入力が数字だった時
                        {
                            var nowData = cleanDataList.GetCleanPlaceData(nowTargetIndex);
                            if (nowData.CheckHaveTarget())
                            {
                                nowData.SetCleanIntervalDate(num);
                                ChangeMode(CurrentMode.DATAUPDATETOPLACEDATA);
                                ResetInputData();
                            }
                            else
                            {
                                WaitInput();
                                ResetInputData();
                            }


                        }
                        else//入力が数字以外だった場合
                        {
                            var nowData = cleanDataList.GetCleanPlaceData(nowTargetIndex);
                            if (!nowData.SetTarget(inputData))
                            {
                                Debug.Log("error Input : " + inputData);
                            }
                            ResetInputData();
                            WaitInput();
                        }
                        break;
                    }
                case CurrentMode.RESET://掃除完了ボタンを押した時
                    {
                        var nowData = cleanDataList.GetCleanPlaceData(nowTargetIndex);
                        nowData.ResetLastUpdateTime();
                        ChangeMode(CurrentMode.DATAUPDATETOPLACEDATA);
                        break;
                    }
                case CurrentMode.CHANGE://変更ボタンを押した時
                    {
                        if (inputData == "placeData")//バツボタンでもどる
                        {
                            ChangeMode(CurrentMode.PLACEDATAMODE);
                            ResetInputData();
                        }
                        else if (inputData == "interval")//時間間隔の変更へ
                        {
                            ChangeMode(CurrentMode.SETINTERVALMODE);
                            ResetInputData();
                            WaitInput();
                        }
                        else if (inputData == "removecheck")//削除の確認へ
                        {
                            ChangeMode(CurrentMode.REMOVECHECK);
                            ResetInputData();
                            WaitInput();
                        }
                        else if (inputData == "rename")//名前の変更へ
                        {
                            ChangeMode(CurrentMode.RENAME);
                            ResetInputData();
                            WaitInput();
                        }
                        break;
                    }
                case CurrentMode.RENAME:
                    {

                        if (inputData == "placeData")
                        {
                            ChangeMode(CurrentMode.PLACEDATAMODE);
                            ResetInputData();
                        }
                        else if (inputData != "")
                        {
                            ChangeMode(CurrentMode.DATAUPDATETOPLACEDATA);
                            cleanDataList.RenamePlaceList(inputData, nowTargetIndex);
                        }
                        else
                        {
                            WaitInput();
                        }
                        break;
                    }
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
            SetTargetIndex(0);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SetInputData("interval");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetInputData("reset");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetInputData("change");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetInputData("removecheck");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SetInputData("dataUpdateToPlaceData");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetInputData("rename");
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
                ResetInputData();
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
    /// 現在のplaceDataの番号
    /// </summary>
    /// <param name="i"></param>
    protected void SetTargetIndex(int i)
    {
        nowTargetIndex = i;
    }

    protected void ResetTargetIndex()
    {
        nowTargetIndex = -1;
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

    /// <summary>
    /// データの初期化（デバッグボタンで呼ぶ）
    /// </summary>
    public void InitData()
    {
        dataSave.InitData<CleanDataList>(cleanDataListPath);
    }

    [ContextMenu("testTimeLimit")]
    public void Debug_testLimit()
    {
        var nowData = cleanDataList.GetCleanPlaceData(nowTargetIndex);
        //var data= nowData.CleanInterval.DayDataUntilNextClean(nowData.CleanInterval,nowData.LastUpdateTime);
        //Debug.Log(data);
    }
}
