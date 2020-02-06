using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//掃除のバーの色と長さの変更を実行するクラス
public class Colorbar : MonoBehaviour
{
    //_safeColor2はいらないかもしれない
    public Color _overTimeColor, _soonClenaColor, _safeColor, _safeColor2;
    private Image cleanBar;
    

    public void ChangeColor(CleanPlaceData data)
    {
        //cleanBarの取得==========================================
        if (cleanBar == null)
        {
            cleanBar = gameObject.GetComponent<Image>();
        }

        //色の決定======================================================

        //次の掃除までの経過割合
        //最後に掃除してからの経過時間/掃除間隔
        float pastTimeRatio = data.FloatLastCleanPassTime()/data.FloatCleanInterval();

        //規格化
        //範囲の最小値が0で最大値が1になるような値
        float normalizedValue = 0;
        //(1/0.25f)
        float extendValue = 4f;

        if (pastTimeRatio > 0.75f)
        {
            normalizedValue = (pastTimeRatio - 0.75f) * extendValue;
            cleanBar.color = Color.Lerp(_soonClenaColor, _overTimeColor, normalizedValue );
        }
        else if (pastTimeRatio > 0.50f)
        {
            normalizedValue = (pastTimeRatio - 0.50f) * extendValue;
            cleanBar.color = Color.Lerp(_safeColor, _soonClenaColor,normalizedValue);
        }
        else if (pastTimeRatio > 0.25f)
        {
            normalizedValue = (pastTimeRatio - 0.25f) * extendValue;
            cleanBar.color = Color.Lerp(_safeColor2, _safeColor, normalizedValue);
        }
        else
        {
            cleanBar.color = _safeColor2;
        }

        //長さの決定===================================================
        cleanBar.fillAmount = pastTimeRatio;
    }

}