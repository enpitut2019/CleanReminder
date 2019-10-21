using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayOutTextList : MonoBehaviour
{
    [SerializeField] GameObject textPrefab;//生成するtext
    RectTransform rtr;
    List<GameObject> textButtonList=new List<GameObject>();//ボタンのリスト
    [SerializeField] Main_UI main_ui;//OpenPlaceDataを取る用

    private void Awake()
    {
        rtr = GetComponent<RectTransform>();
    }
    

    /// <summary>
    /// textをLayOutに追加する
    /// </summary>
    /// <param name="st"></param>
    public void AddText(string st)
    {
        var obj = Instantiate(textPrefab, rtr.position, Quaternion.identity);
        textButtonList.Add(obj);

        //親の設定処理
        var objRtr = obj.GetComponent<RectTransform>();
        objRtr.SetParent(rtr);
        objRtr.localScale = new Vector3(1, 1, 1);

        //テキストの書き換え
        var t= obj.GetComponentInChildren<Text>();
        t.text = st;

        //ボタンに関数を追加
        var button = obj.GetComponent<Button>();
        int temp = textButtonList.Count - 1;
        button.onClick.AddListener(()=>main_ui.OpenPlaceDataMode(temp));

    }

    /// <summary>
    /// textの削除
    /// </summary>
    /// <param name="i"></param>
    public void RemoveText(int i)
    {
        Destroy(textButtonList[i]);
        textButtonList.RemoveAt(i);
    }

    [ContextMenu("resetText")]
    public void ResetText()
    {
        for(int i = textButtonList.Count - 1; i >= 0; i--)
        {
            RemoveText(i);
        }
    }


    [ContextMenu("addText")]
    public void Test_AddText()
    {
        AddText("unko"+textButtonList.Count);
    }


    [ContextMenu("removeText")]
    public void Test_removeAt()
    {
        RemoveText(0);
    }

}
