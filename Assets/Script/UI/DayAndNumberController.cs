using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IRecieveDayAndNumber {
    /// <summary>
    /// initDropDown のDayとnumerを受け取る関数
    /// </summary>
    /// <param name="Day"></param>
    /// <param name="number"></param>
     void RecieveDayAndNumberAction(string Day, int number);
}

/// <summary>
/// intervalDataに必要なDayとNumberの情報を持っているクラス
/// </summary>
public class DayAndNumberController : MonoBehaviour
{
    [SerializeField] Dropdown numberDrop;//対象のDropDown
    [SerializeField] Dropdown dayDrop;//対象のDropDown
    [SerializeField] Vector2Int setNum;//InitDropで設定する範囲

    public string DayDropText { get { return dayDrop.captionText.text; } }//daydropの現在取得中のtext
    public string NumberDropText { get { return numberDrop.captionText.text; } }//numberdropの現在取得中のtext

    [SerializeField] GameObject actionTarget;//buttonを押したときに呼び出される変数を実装している奴
    IRecieveDayAndNumber actionInterface;

    [SerializeField] bool isDebug=false;

    private void Start()
    {
        actionInterface = actionTarget.GetComponent<IRecieveDayAndNumber>();
        if (actionInterface == null)
        {
            Debug.Log("Error InitDropDown : gameObject not have RecieveDayAndNumber interface");
        }
        //InitDayDropDown();
    }

    /// <summary>
    /// アクティブになった時の初期化処理
    /// </summary>
    private void OnEnable()
    {
        numberDrop.value = 0;
        dayDrop.value = 0;
    }

    public void SetDropDownRangeDay()
    {
        string text = dayDrop.captionText.text;
        if (text == "Year")
        {
            SetNumberDropDownRange(10);
        }
        else if (text == "Month")
        {
            SetNumberDropDownRange(12);
        }else if (text == "Week")
        {
            SetNumberDropDownRange(4);
        }
        else if (text == "Day")
        {
            SetNumberDropDownRange(30);
        }else if(text == "Minute")
        {
            SetNumberDropDownRange(30);
        }else if(text == "Second")
        {
            SetNumberDropDownRange(30);
        }
        else
        {
            SetNumberDropDownRange(0);
        }
    }

    /// <summary>
    /// ドロップダウンの数を指定した数にする
    /// </summary>
    /// <param name="range"></param>
    public void SetNumberDropDownRange(int range)
    {
        //numberDrop.options = new List<Dropdown.OptionData>();
        numberDrop.options.Clear();
        for (int i = 0; i <= range; i++)
        {
            numberDrop.options.Add(new Dropdown.OptionData(i.ToString()));
        }
        numberDrop.value = 0;
    }

    /// <summary>
    /// daydropdownの初期化+isDebug==tureならばsecond,minuteも表示s
    /// </summary>
    [ContextMenu ("initDropDOwn")]
    void InitDayDropDown()
    {
        dayDrop.options.Clear();

        dayDrop.options.Add(new Dropdown.OptionData("-------"));
        if (isDebug)
        {
            dayDrop.options.Add(new Dropdown.OptionData("Second"));
            dayDrop.options.Add(new Dropdown.OptionData("Minute"));
        }
        dayDrop.options.Add(new Dropdown.OptionData("Day"));
        dayDrop.options.Add(new Dropdown.OptionData("Month"));
        dayDrop.options.Add(new Dropdown.OptionData("Year"));
        dayDrop.value = 0;


    }

    /// <summary>
    /// ボタンでactionTargetの関数を呼び出す関数
    /// </summary>
    public void SendDayAndNumber_button()
    {
        string day = DayDropText;
        //weekをDayに変換する処理
        int num = int.Parse(NumberDropText);
        if (day == "Week")
        {
            day = "Day";
            num *= 7;

        }
        actionInterface.RecieveDayAndNumberAction(day,num);
    }

    [ContextMenu("setIntRange")]
    public void SetIntRange()
    {
        numberDrop.options = new List<Dropdown.OptionData>();
        for(int i = setNum.x; i <= setNum.y; i++)
        {
            numberDrop.options.Add(new Dropdown.OptionData(i.ToString()));
        }
    }

    [ContextMenu("reset")]
    public void ResetDrop()
    {
        numberDrop.options = new List<Dropdown.OptionData>();
    }
}
