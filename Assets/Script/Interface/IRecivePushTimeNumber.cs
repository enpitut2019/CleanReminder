using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecivePushTimeNumber
{
    /// <summary>
    /// 通地時刻を設定する数字を送るinterface
    /// PushTimeSetterから送られる
    /// </summary>
    /// <param name="num"></param>
    void RecivePushTimeNumber(string num);
}
