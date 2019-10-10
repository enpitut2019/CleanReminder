using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_UI : MainBase
{
    protected override void CurrentUpdate_Add()
    {
        base.CurrentUpdate_Add();
        Debug.Log("add");
    }

    protected override bool Inputaa()
    {
        return Input.anyKeyDown;
    }
}
