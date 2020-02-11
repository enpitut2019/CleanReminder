using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModeStack
{
    [SerializeField] Stack<MainBase.CurrentMode> modeStack=new Stack<MainBase.CurrentMode>();

    //nextModeまでpopする
    //popしたもののリストを返す
    public List<MainBase.CurrentMode> ToPop(MainBase.CurrentMode nextMode)
    {
        var resultList = new List<MainBase.CurrentMode>();
        if (!modeStack.Contains(nextMode))
        {
            Debug.Log("stack ni haittenai yo");
            return null;
        }

        while (true)
        {
            var targetMode = modeStack.Peek();
            if (targetMode == nextMode)
            {
                return resultList;
            }
            else
            {
                resultList.Add(modeStack.Pop());
            }
        }
    }

    public void Push(MainBase.CurrentMode mode)
    {
        modeStack.Push(mode);
    }
}
