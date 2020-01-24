using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCleanPlaceData : MonoBehaviour
{
    CleanPlaceData myData;
    [SerializeField] Text nextCleanTime;
    [SerializeField] Text intervalTime;
    [SerializeField] Text placeName;

    Animator animator;

    //一瞬非アクティブになる場合
    [SerializeField] bool animType_awake;
    bool awakeTrigger=false;

    [SerializeField] Main_UI main;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //一瞬非アクティブになる問題への対応
        //animType_awakeでOnOffができる
        //無理やり感あるので直したい
        if (awakeTrigger && animType_awake)
        {
            animator.SetTrigger("Finish");
            awakeTrigger = false;
        }
    }

    /// <summary>
    /// 表示するデータの登録
    /// </summary>
    /// <param name="data"></param>
    public void SetCleanPlaceData(CleanPlaceData data)
    {
        myData = data;
    }

    /// <summary>
    /// データをテキストに表示する関数
    /// </summary>
    public void DisplayData()
    {
        if (myData != null)
        {
            nextCleanTime.text = myData.NextCleanLeftTimeText;
            intervalTime.text = myData.CleanIntervalText;
            placeName.text = myData.Place;
        }
    }

    
    public void Animation_CleanFinish()
    {
        if (animType_awake)//一瞬非アクティブになる場合
        {
            awakeTrigger = true;
        }
        else
        {
            animator.SetTrigger("Finish");
        }
    }

    public void AnimEvent_ChengeDisplayMode(){
        main.ChangeDisplayMode();
    }
}
