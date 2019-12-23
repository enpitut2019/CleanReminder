using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAdditional_setDropDownRange : MonoBehaviour
{
    [SerializeField] Text SourceDaytext;
    [SerializeField] Text SourceNumberText;

    [SerializeField] Dropdown ChengeDayDropDown;
    [SerializeField] Dropdown ChengeNumDropDown;


    public void SetDropDownRange()
    {
        string text = SourceDaytext.text;
        string numText = SourceNumberText.text;
        int rangeDayNum = -1;
        int.TryParse(numText, out rangeDayNum);
        if (rangeDayNum == -1)
        {
            SetNumberDropDownRange(0);
            return;
        }
        else
        {

        }
        int numDayRate = 1;
        if (text == "Day")
        {

        }else if (text=="Week")
        {
            numDayRate = 7;
        }
        else if(text == "Month")
        {
            numDayRate = 30;
        }
        rangeDayNum *= numDayRate;

        if (ChengeDayDropDown.captionText.text== "Day")
        {
            if (rangeDayNum > 30) rangeDayNum = 30;
            SetNumberDropDownRange(rangeDayNum);
        }
        else if (ChengeDayDropDown.captionText.text == "Week")
        {
            int rangeWeek = rangeDayNum / 7;
            if (rangeWeek > 4) rangeWeek = 4;
            SetNumberDropDownRange(rangeWeek);
        }
        else if (ChengeDayDropDown.captionText.text == "Month")
        {
            int rangeMonth = rangeDayNum / 30;
            if (rangeMonth > 12) rangeMonth = 12;
            SetNumberDropDownRange(rangeMonth);
        }
    }


    /// <summary>
    /// ドロップダウンの数を指定した数にする
    /// </summary>
    /// <param name="range"></param>
    public void SetNumberDropDownRange(int range)
    {
        //numberDrop.options = new List<Dropdown.OptionData>();
        ChengeNumDropDown.options.Clear();
        for (int i = 0; i <= range; i++)
        {
            ChengeNumDropDown.options.Add(new Dropdown.OptionData(i.ToString()));
        }
        if (ChengeNumDropDown.value > ChengeNumDropDown.options.Count)
            ChengeNumDropDown.value = 0;
    }

}
