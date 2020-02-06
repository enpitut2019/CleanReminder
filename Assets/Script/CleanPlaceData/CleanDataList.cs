using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 掃除場所のデータを保持するクラス
/// </summary>
[System.Serializable]
public class CleanDataList
{
    //pushTiming関係が大きくなったら分ける
    #region pushTiming関連　
    [SerializeField] int pushTiming = 18;//push通知を送るタイミングのセーブデータ
    public int PushTiming { get { return pushTiming; } }
    /// <summary>
    /// プッシュタイミングの変更
    /// </summary>
    /// <param name="time"></param>
    public void SetPushTIiming(int time)
    {
        pushTiming = time;
    }
    #endregion


    /// <summary>
    /// 場所のデータ
    /// </summary>
    public List<CleanPlaceData> placeDataList = new List<CleanPlaceData>();


    /// <summary>
    /// 場所のデータの追加
    /// </summary>
    /// <param name="placename"></param>
    public void AddPlaceList(string placename)
    {
        //var data = new CleanPlaceData();
        //placeDataList.Add(placename);
        placeDataList.Add(new CleanPlaceData(placename));
    }

    public void AddPlaceList(CleanPlaceData data)
    {
        placeDataList.Add(data);
        //Debug.Log("=========================data : "+data.LastUpdateTime);
    }

    public void RenamePlaceList(string placename, int index)
    {
        //placeDataList[index].Place = placename;                   aaaaaaa
        CleanPlaceData data = placeDataList[index];
        data.SetPlaceName(placename);
        //placeDataList[index].SetPlaceName(placename);
    }

    /// <summary>
    /// 場所のデータの取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetPlaceData(int index)
    {
        return placeDataList[index].Place;
    }

    public string GetDateData(int index)
    {
        return placeDataList[index].LastUpdateTimeText;
    }

    /// <summary>
    /// CleanPlaceDataを丸ごと取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public CleanPlaceData GetCleanPlaceData(int index)
    {
        return placeDataList[index];
    }

    /// <summary>
    /// 場所のデータの削除
    /// </summary>
    /// <param name="index"></param>
    public void RemoveData(int index)
    {
        if (placeDataList.Count > index)
        {
            placeDataList.RemoveAt(index);
        }
    }

    /// <summary>
    /// 締め切り（次の掃除までの残り期間）が近い順にソート
    /// </summary>
    /// <returns></returns>
    public void DeadLineSort()
    {
        placeDataList.Sort((a,b) => a.GetLeftDay() - b.GetLeftDay());
    }

}
