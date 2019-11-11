using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// layOutElementの数とパネルのサイズを合わせる
/// </summary>
public class ScrollPanel : MonoBehaviour
{
    [SerializeField] RectTransform strechPanel;
    [SerializeField] VerticalLayoutGroup myLayOut;
    //SerializeField]List<RectTransform> childComponets;
    
    float firstHeight;//自身の最初の高さ
    float spaceing;//子の要素の間隔
    int beforeChildCount=-1;
    bool stated = false;//更新が開始するためのフラグ


    private void Awake()
    {
        firstHeight = strechPanel.sizeDelta.y;
        spaceing = myLayOut.spacing;
        StartCoroutine(StartUpdate(1.0f));
    }
    private void Update()
    {
        if (!stated) return;//フラグが立つまで待機
        if (beforeChildCount != strechPanel.childCount)//要素数が変更されていたら更新
        {
            beforeChildCount = strechPanel.childCount;
            if (beforeChildCount != 0)
            {
                SetPanelLength(beforeChildCount);
            }
        }
    }
    
    /// <summary>
    /// panelの長さの更新
    /// </summary>
    /// <param name="count"></param>
    public void SetPanelLength(int count)
    {
        var compHeight = GetChildHeight();
        var size = strechPanel.sizeDelta;
        size.y = (compHeight+spaceing) * count;
        if (size.y < firstHeight)//初期サイズより小さいなら初期サイズにする
        {
           size.y = firstHeight;
        }
        strechPanel.sizeDelta = size;
        strechPanel.position = GetUpdatePosition();
    }


    /// <summary>
    /// 子要素のｙの長さ
    /// </summary>
    /// <returns></returns>
    public float GetChildHeight()
    {
        return strechPanel.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
    }
    /// <summary>
    /// 更新後の位置を取得
    /// </summary>
    /// <returns></returns>
    Vector2 GetUpdatePosition()
    {
        return new Vector2(0, (strechPanel.sizeDelta.y - firstHeight) / 2.0f);
    }

    /// <summary>
    /// 指定秒後に更新開始
    /// 初期化がうまくいかなかったので
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    IEnumerator StartUpdate(float f)
    {
        yield return new WaitForSeconds(f);
        stated = true;
    }
}
