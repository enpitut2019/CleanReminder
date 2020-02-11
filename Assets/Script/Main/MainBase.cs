using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        REMOVECHECK,//削除するか確認する状態
        REMOVE,//データを削除する状態
        PLACEDATAMODE,//場所のデータの詳細を表示している状態
        SETINTERVALMODE,//掃除する間隔の登録をする状態
        CHANGE,//変更ボタンを押して何を変更するか選択する状態
        RENAME,//名前を変更する状態
        OPTION//オプション設定画面 現在は通知時刻設定のみ
    }

    [SerializeField] protected CurrentMode _currentMode = CurrentMode.DISPLAY;
    [SerializeField] protected CleanDataList cleanDataList = new CleanDataList();//掃除場所のデータリストを扱うクラス
    [SerializeField] List<string> inputDataList = new List<string>();//受け取った入力を入れるリスト
    [SerializeField] string inputDataTop { get { return inputDataList[0]; } }//受け取った入力を受け取るリストの先頭
    [SerializeField] bool canInput;//入力受け取り状態を表す変数

    [SerializeField] protected int nowTargetIndex = -1;//MainBaseに実装を映したい
    CleanPlaceData makingNowData;//作成中のデータin ADDPLACEMODE 終了後空っぽになる
    

    [SerializeField] protected PushController pushCtrl;//push通知を送ったりするクラス

    [SerializeField] ModeStack _modeStack = new ModeStack();

    [SerializeField] CurrentMode? _animCoalMode = null;
    [SerializeField] CurrentMode? _animedChengeMode = null;
    bool _AnimMode { get { return _animCoalMode != null && _animedChengeMode != null; } }
    UnityEvent animCoaledEvent = new UnityEvent();//アニメーションが終わった後に呼ばれる関数

    #region データをセーブするpath群
    string cleanDataListPath = "cleanPlaceData";
    #endregion

    CurrentMode nextMode;
    #region Monobehabiour関数
    void Start()
    {
        LoadCleanDataList();
        foreach(CleanPlaceData placeData in cleanDataList.placeDataList)
        {
            placeData.InitAction();
        }
        pushCtrl.SetPushTime(cleanDataList.PushTiming);
        cleanDataList.DeadLineSort();
        ChangeMode(CurrentMode.DISPLAY,addOpen:true);
    }

    void Update()
    {
        if (canInput)
        {
            InputUpdate();
        }
        else
        {
            switch (_currentMode)//currentModeごとにUpdate関数を呼び出す
            {
                case CurrentMode.DISPLAY:
                    if (inputDataTop == "i")
                    {
                        ChangeMode(CurrentMode.ADDPLACEMODE, addOpen: true);
                    }
                    else if (inputDataTop == "placeData")
                    {
                        
                        ChangeMode(CurrentMode.PLACEDATAMODE, addOpen: true);
                    }
                    else if(inputDataTop=="option")
                    {
                        ChangeMode(CurrentMode.OPTION, addOpen: true);
                    }
                    else
                    {
                        ReInput(reset: false);
                    }
                    break;
                case CurrentMode.ADDPLACEMODE:
                    {
                        if (inputDataTop == "display")
                        {
                            ChangeMode(CurrentMode.DISPLAY, addOpen: false);
                        }else if (inputDataList.Count == 5)
                        {
                            Debug.Log(inputDataList[0]);
                            if (inputDataList[0] == "")
                            {
                                ReInput(reset: true);
                                break;
                            }
                            var localData = new CleanPlaceData(inputDataList[0]);
                            if(!localData.SetTarget(inputDataList[1]) || !localData.SetTarget(inputDataList[3]) || int.Parse(inputDataList[2]) == 0) //setIntervalのターゲット（day,month,year）
                            {
                                Debug.Log("error Input datalist1: " + inputDataList[1]);
                                Debug.Log("error Input datalist1: " + inputDataList[3]);
                                ReInput(reset: true);
                                break;
                            }
                            int intervalRate = 0;
                            if(inputDataList[1] == "Day")
                            {
                                intervalRate = 1;
                            }
                            else if(inputDataList[1] == "Month")
                            {
                                intervalRate = 30;
                            }
                            else if(inputDataList[1] == "Year")
                            {
                                intervalRate = 365;
                            }
                            var num = int.Parse(inputDataList[2]);
                            localData.SetCleanIntervalDate(num);


                            int intervalRate_next = 0;
                            System.DateTime time = System.DateTime.Now;
                            if (inputDataList[3] == "Day")
                            {
                                intervalRate_next = 1;
                            }
                            else if(inputDataList[3] == "Month")
                            {
                                intervalRate_next = 30;
                            }
                            else if(inputDataList[3] == "Year")
                            {
                                intervalRate_next = 365;
                            }
                            
                            time = time.AddDays(- intervalRate * int.Parse(inputDataList[2]) + intervalRate_next * int.Parse(inputDataList[4]));
                            localData.SetLastUpdateTime(time);
                            cleanDataList.AddPlaceList(localData);
                            ChangeModeUpdate(CurrentMode.DISPLAY, addOpen: false);
                        }

                        break;
                    }
                case CurrentMode.REMOVECHECK:
                    {
                        if (inputDataTop == "remove")//削除ボタンを押した時
                        {
                            ChangeMode(CurrentMode.REMOVE, addOpen: true);
                        }
                        else if (inputDataTop == "placeData")//キャンセルボタンを押した時
                        {
                            ChangeMode(CurrentMode.PLACEDATAMODE, addOpen: false);
                        }
                        break;
                    }
                case CurrentMode.REMOVE:
                    {
                        int num = 0;
                        bool result = int.TryParse(inputDataTop, out num);
                        if (result)//入力が数字だった時
                        {
                            cleanDataList.RemoveData(num);
                            ChangeModeUpdate(CurrentMode.DISPLAY, addOpen: false);
                        }
                        else//入力が数字以外だった場合
                        {
                            ReInput(reset: true);
                        }

                        break;
                    }
                case CurrentMode.PLACEDATAMODE:
                    if (inputDataTop == "test")
                    {
                        var nowData = cleanDataList.GetCleanPlaceData(nowTargetIndex);
                        ReInput(reset: false);
                    }

                    if (inputDataTop == "display")
                    {
                        ChangeMode(CurrentMode.DISPLAY, addOpen: false);
                    }
                    //else if (inputDataTop == "interval")
                    //{
                    //    ChangeMode(CurrentMode.SETINTERVALMODE, addOpen: true);
                    //}
                    else if (inputDataTop == "reset")
                    {
                        var nowData = cleanDataList.GetCleanPlaceData(nowTargetIndex);
                        nowData.ResetLastUpdateTime();

                        SetAnimMode(CurrentMode.PLACEDATAMODE, CurrentMode.DISPLAY);
                        ChangeModeUpdate(CurrentMode.DISPLAY, addOpen: false);
                    }
                    else if (inputDataTop == "change")
                    {
                        ChangeMode(CurrentMode.CHANGE, addOpen: true);
                    }
                    break;
                case CurrentMode.SETINTERVALMODE:
                    {
                        int num = 0;
                        bool result = int.TryParse(inputDataTop, out num);
                        if (inputDataTop == "placeData")//キャンセルボタンを押した時
                        {
                            ChangeMode(CurrentMode.PLACEDATAMODE, addOpen: false);
                        }
                        else if (result)//入力が数字だった時
                        {
                            var nowData = cleanDataList.GetCleanPlaceData(nowTargetIndex);
                            if (nowData.CheckHaveTarget())
                            {
                                nowData.SetCleanIntervalDate(num);
                                ChangeModeUpdate(CurrentMode.PLACEDATAMODE, addOpen: false);
                            }
                            else
                            {
                                ReInput(reset: true);
                            }


                        }
                        else//入力が数字以外だった場合
                        {
                            var nowData = cleanDataList.GetCleanPlaceData(nowTargetIndex);
                            if (!nowData.SetTarget(inputDataTop))
                            {
                                Debug.Log("error Input : " + inputDataTop);
                            }
                            ReInput(reset: true);
                        }
                        break;
                    }
                case CurrentMode.CHANGE://変更ボタンを押した時
                    {
                        if (inputDataTop == "placeData")//バツボタンでもどる
                        {
                            ChangeMode(CurrentMode.PLACEDATAMODE, addOpen: false);
                        }
                        else if (inputDataTop == "interval")//時間間隔の変更へ
                        {
                            ChangeMode(CurrentMode.SETINTERVALMODE, addOpen: true);
                        }
                        else if (inputDataTop == "removecheck")//削除の確認へ
                        {
                            ChangeMode(CurrentMode.REMOVECHECK, addOpen: true);
                        }
                        else if (inputDataTop == "rename")//名前の変更へ
                        {
                            ChangeMode(CurrentMode.RENAME, addOpen: true);
                        }
                        break;
                    }
                case CurrentMode.RENAME:
                    {

                        if (inputDataTop == "placeData")
                        {
                            ChangeMode(CurrentMode.PLACEDATAMODE, addOpen: false);
                        }
                        else if (inputDataTop != "")
                        {
                            //順序逆だとエラーを吐きました
                            cleanDataList.RenamePlaceList(inputDataTop, nowTargetIndex);
                            ChangeModeUpdate(CurrentMode.PLACEDATAMODE, addOpen: false);
                        }
                        else
                        {
                            ReInput(reset: true);
                        }
                        break;
                    }
                case CurrentMode.OPTION:
                    {
                        int num = 0;
                        bool result = int.TryParse(inputDataTop, out num);
                        if (inputDataTop == "display")
                        {
                            ChangeModeUpdate(CurrentMode.DISPLAY, addOpen: false);
                        }
                        else if (result)//入力が数字だった時
                        {
                            pushCtrl.SetPushTime(num);
                            cleanDataList.SetPushTIiming(num);
                            ReInput(reset: true);
                        }
                        else//入力が数字以外だった場合
                        {
                            ReInput(reset: true);
                        }
                    }
                    break;
            }
        }

    }
    #endregion


    #region mode関連の関数
    /// <summary>
    /// currentModeの変更
    /// </summary>
    /// <param name="nextMode"></param>
    void ChangeMode(CurrentMode nextMode,bool addOpen)
    {
        //Debug.Log("addOpen"+addOpen);

        if (_currentMode == nextMode)
        {
            Debug.Log("おなじモードが呼び出されています");
            return;
        }

        if (addOpen)
        {
            _modeStack.Push(nextMode);
            AwakeModeAction(nextMode);
        }
        else
        {

            if (_AnimMode )
            {
                AnimCoalPop(addOpen);
                return;
            }

            var popList= _modeStack.ToPop(nextMode);
            foreach(var pop in popList)
            {
                EndModeAction(pop);
            }
            AwakeModeAction(nextMode);
        }

        //現在のモードの終了処理を行い、次のモードの開始処理を行う
        //EndModeAction(_currentMode);
        //AwakeModeAction(nextMode);

        _currentMode = nextMode;

        ResetInputData();
        WaitInput();
    }

    //最後に呼ばないとエラーを吐く？
    //名前変更時にエラーを吐いた
    //モード変更の際にデータのセーブも行う
    protected virtual void ChangeModeUpdate(CurrentMode nextMode,bool addOpen)
    {
        //同じモードを呼び出したときの挙動が不安
        //この処理は　reset入力時の　PlaceDataMode->PlaceDataMode　で必要になった
        //animationの処理を頑張れば解決できそう
        if (_currentMode == nextMode)
        {
            ResetInputData();
            WaitInput();
        }
        else
        {
            ChangeMode(nextMode,addOpen);
        }

        SaveCleanDataList();
    }

    //再入力をする
    void ReInput(bool reset)
    {
        if (reset) ResetInputData();
        WaitInput();
    }

    /// <summary>
    /// モードが終了したときの処理
    /// </summary>
    /// <param name="mode"></param>
    protected virtual void EndModeAction(CurrentMode mode)
    {
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
                Debug.Log(_currentMode);
                ResetInputData();
                break;
            case CurrentMode.ADDPLACEMODE:
                Debug.Log(_currentMode);
                break;
        }
    }

    //animModeの時にmodeChengeで呼ばれる関数
    //animEndまで閉じてanimの処理を実行
    void AnimCoalPop(bool addOpen)
    {

        var popList = _modeStack.ToPop((CurrentMode)_animCoalMode);
        foreach (var pop in popList)
        {
            EndModeAction(pop);
        }

        animCoaledEvent.AddListener(() => ChangeMode((CurrentMode)_animedChengeMode, addOpen));//anim終了後の関数を登録
        AnimCoal((CurrentMode)_animCoalMode);//アニメーション呼び出し
    }

    #endregion

    #region 入力関連の関数
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
    /// キー入力でデータを入力するための関数
    /// デバック用
    /// </summary>
    void InputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Enter();//入力の確定
        }

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    AddInputData("i");//AddPlaceModeに入るための文字列の入力
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    AddInputData("display");//DisPlayModeに入るための文字列の入力
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    AddInputData("remove");
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    AddInputData("placeData");
        //    SetTargetIndex(0);
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    AddInputData("interval");
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    AddInputData("reset");
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    AddInputData("change");
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    AddInputData("removecheck");
        //}
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    AddInputData("dataUpdateToPlaceData");
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    AddInputData("rename");
        //}
    }

    /// <summary>
    /// inputDataの追加
    /// </summary>
    /// <param name="data"></param>
    protected virtual void AddInputData(string data)
    {
        inputDataList.Add(data);
    }

    /// <summary>
    /// inputDataの初期化
    /// </summary>
    protected virtual void ResetInputData()
    {
        inputDataList = new List<string>();
    }
    #endregion

    #region 現在のインデックスについて
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
    #endregion
    #region Save&Load関連の関数
    /// <summary>
    /// データをjsonファイルに書き込む
    /// </summary>
    void SaveCleanDataList(){
        DataSaveClass.SaveData<CleanDataList>(cleanDataList, cleanDataListPath);
    }

    /// <summary>
    /// データをjsonファイルから読み込み
    /// </summary>
    void LoadCleanDataList(){
        cleanDataList=DataSaveClass.LoadData<CleanDataList>(cleanDataListPath);
    }
    #endregion

    #region animation

    //アニメーションを呼ぶ状態を設定する関数
    //ChengeModeを呼ぶ前に呼んでおく
    void SetAnimMode(CurrentMode animCoalMode, CurrentMode animNextMode)
    {
        _animCoalMode = animCoalMode;
        _animedChengeMode = animNextMode;
    }
    //アニメーションを読んだ後にアニメーションイベントで呼ばれる関数
    public void AnimationEvent_CoalEndAnimation()
    {
        _animCoalMode = null;
        animCoaledEvent.Invoke();
        animCoaledEvent = new UnityEvent();
        _animedChengeMode = null;
    }

    //アニメーションを実際に呼ぶ関数
    //ChengeMode内で呼ばれる
    public virtual void AnimCoal(CurrentMode coalAnimMode)
    {
        switch (coalAnimMode)
        {
            case CurrentMode.PLACEDATAMODE:

                break;
            default:
                Debug.Log("not set animation data");
                break;
        }
    }
    #endregion

    #region　不要かもしれない関数
    //検証が不十分なためコメントアウトした


    /// <summary>
    /// データの初期化（デバッグボタンで呼ぶ）
    /// </summary>
    //public void InitData()
    //{
    //    dataSave.InitData<CleanDataList>(cleanDataListPath);
    //}


    //[ContextMenu("testTimeLimit")]
    //public void Debug_testLimit()
    //{
    //    var nowData = cleanDataList.GetCleanPlaceData(nowTargetIndex);
    //    //var data= nowData.CleanInterval.DayDataUntilNextClean(nowData.CleanInterval,nowData.LastUpdateTime);
    //    //Debug.Log(data);
    //}



    ///// <summary>
    ///// nフレーム後にuaを実行
    ///// </summary>
    ///// <param name="n"></param>
    ///// <param name="ua"></param>
    ///// <returns></returns>
    //public IEnumerator WaitFrame(int n, UnityAction ua)
    //{
    //    for (int i = 0; i < n; i++)
    //    {
    //        yield return null;
    //    }
    //    ua.Invoke();
    //}
    #endregion
}
