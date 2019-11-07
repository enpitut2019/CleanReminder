using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ドロップダウンを初期化するためのクラス
/// </summary>
public class InitDropDown : MonoBehaviour
{
    [SerializeField] Dropdown drop;//対象のDropDown
    [SerializeField] Vector2Int setNum;//InitDropで設定する範囲

    [ContextMenu("setIntRange")]
    public void SetIntRange()
    {
        drop.options = new List<Dropdown.OptionData>();
        for(int i = setNum.x; i <= setNum.y; i++)
        {
            drop.options.Add(new Dropdown.OptionData(i.ToString()));
        }
    }

    [ContextMenu("reset")]
    public void ResetDrop()
    {
        drop.options = new List<Dropdown.OptionData>();
    }
}
