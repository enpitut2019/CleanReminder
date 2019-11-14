using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ドロップダウンを初期化するためのクラス
/// </summary>
public class InitDropDown : MonoBehaviour
{
    [SerializeField] Dropdown numberDrop;//対象のDropDown
    [SerializeField] Dropdown dayDrop;//対象のDropDown
    [SerializeField] Vector2Int setNum;//InitDropで設定する範囲

    public void SetDropDownRangeDay()
    {
        string text = dayDrop.captionText.text;
        if (text == "Year")
        {
            SetDropDownRange(10);
        }
        else if (text == "Month")
        {
            SetDropDownRange(12);
        }
        else if (text == "Day")
        {
            SetDropDownRange(30);
        }
        else
        {
            SetDropDownRange(0);
        }
    }

    /// <summary>
    /// ドロップダウンの数を指定した数にする
    /// </summary>
    /// <param name="range"></param>
    public void SetDropDownRange(int range)
    {
        //numberDrop.options = new List<Dropdown.OptionData>();
        numberDrop.options.Clear();
        for (int i = 0; i <= range; i++)
        {
            numberDrop.options.Add(new Dropdown.OptionData(i.ToString()));
        }
        numberDrop.value = 0;
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
