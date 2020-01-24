using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class SEDataTime
{
    [SerializeField] int year;
    [SerializeField] int month;
    [SerializeField] int day;
    [SerializeField] int hour;
    [SerializeField] int minute;
    [SerializeField] int second;
    string targetkey = "";
    Dictionary<string, int> dataTimeDictionary;



    public SEDataTime(DateTime time)
    {
        this.year = time.Year;
        this.month = time.Month;
        this.day = time.Day;
        this.hour = time.Hour;
        this.minute = time.Minute;
        this.second = time.Second;
    }

    public SEDataTime(TimeSpan time)
    {
        //var day_temp = time.Days;
        this.year = 0;
        this.month = 0;
        /*while (day_temp > 365)
        {
            day_temp -= 365;
            this.year += 1;
        }

        while (day_temp > 30)
        {
            day_temp -= 30;
            this.month += 1;
        }*/

        this.day = time.Days;
        this.hour = time.Hours;
        this.minute = time.Minutes;
        this.second = time.Seconds;
    }

    public SEDataTime(string Day, int number)
    {
        ChangeTarget(Day);
        ChangeDate(number);
    }

    public SEDataTime()
    {
        this.year = 0;
        this.month = 0;
        this.day = 0;
        this.hour = 0;
        this.minute = 0;
        this.second = 0;
    }

    public bool ChangeTarget(string key)
    {
        if (dataTimeDictionary == null)
        {
            InitDictionary();
        }
        if (dataTimeDictionary.ContainsKey(key))
        {
            targetkey = key;
            return true;
        }
        targetkey = "";
        return false;
    }

    public bool CheackHaveTarget()
    {
        return !(targetkey == "");
    }

    public void ChangeDate(int value)
    {
        if (dataTimeDictionary == null)
        {
            InitDictionary();
        }
        if (dataTimeDictionary.ContainsKey(targetkey))
        {
            year = 0;
            month = 0;
            day = 0;
            hour = 0;
            minute = 0;
            second = 0;

            //dataTimeDictionary[targetkey] = value;
            switch (targetkey)
            {
                case "Year":
                    year = value;
                    break;
                case "Month":
                    month = value;
                    break;
                case "Day":
                    day = value;
                    break;
                case "Hour":
                    hour = value;
                    break;
                case "Minute":
                    minute = value;
                    break;
                case "Second":
                    second = value;
                    break;
            }
        }
    }

    public void ChangeDate(string key, int value)
    {
        ChangeTarget(key);
        ChangeDate(value);
    }

    public int GetDate(string key)
    {
        if (dataTimeDictionary == null)
        {
            InitDictionary();
        }
        int result = -1;
        if (dataTimeDictionary.ContainsKey(key))
        {
            result = dataTimeDictionary[key];
        }
        return result;
    }

    void InitDictionary()
    {
        dataTimeDictionary = new Dictionary<string, int>();
        dataTimeDictionary.Add("Year", year);
        dataTimeDictionary.Add("Month", month);
        dataTimeDictionary.Add("Day", day);
        dataTimeDictionary.Add("Hour", hour);
        dataTimeDictionary.Add("Minute", minute);
        dataTimeDictionary.Add("Second", second);
    }

    //時間に換算する関数
    public void CalcuToHour()
    {
        DateTime date1 = new DateTime(2010, 1, 1, 8, 0, 15);
        DateTime date2 = new DateTime(2010, 6, 1, 11, 2, 16);
        TimeSpan interval = date2 - date1;
        Debug.Log(interval);
        DateTime date3 = new DateTime(1, 1, 1, 0, 0, 0);
        date3 += interval;
        //Debug.Log(date3);
        interval = new TimeSpan(1, 2, 3);
        date2 += interval;
        //Debug.Log(date2);
    }
    public List<int> OutDayDatas()
    {
        var result = new List<int>();
        result.Add(year);
        result.Add(month);
        result.Add(day);
        result.Add(hour);
        result.Add(minute);
        result.Add(second);
        return result;
    }
}
