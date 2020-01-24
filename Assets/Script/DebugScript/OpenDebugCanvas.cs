using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDebugCanvas : MonoBehaviour
{
    int nowCount = -1;
    [SerializeField] GameObject[] DebugCanvas;

    public void SwichDebugCanvas()
    {
        AddNowCount();
        foreach (var obj in DebugCanvas)
        {
            obj.SetActive(false);
        }
        if (nowCount >= 0)
        {
            DebugCanvas[nowCount].SetActive(true);
        }
    }

    void AddNowCount()
    {
        nowCount++;
        if (nowCount >= DebugCanvas.Length)
        {
            nowCount = -1;
        }
    }
}
