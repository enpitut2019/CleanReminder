using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_dropDown : MonoBehaviour
{
    [SerializeField] Dropdown dropDown;

    public void TestDrop()
    {
        Debug.Log(dropDown.captionText.text);
    }
}
