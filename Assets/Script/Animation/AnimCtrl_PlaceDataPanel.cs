using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlaceDataPanelのアニメーションを呼ぶクラス
//実装が無理やりなのでそのうち改善したい
public class AnimCtrl_PlaceDataPanel : MonoBehaviour
{
    //一瞬非アクティブになる場合
    [SerializeField] bool animType_awake;
    bool awakeTrigger = false;
    
    Animator animator;

   Main_UI main;



    private void Start()
    {
        animator = GetComponent<Animator>();

        //重いので使いたくないです
        main = GameObject.FindObjectOfType<Main_UI>().GetComponent<Main_UI>();
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




    public void AnimEvent_ChengeDisplayMode()
    {
        //main.ChangeDisplayMode();
        main.AnimationEvent_CoalEndAnimation();
    }

}
