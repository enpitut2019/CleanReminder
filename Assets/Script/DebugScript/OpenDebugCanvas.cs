using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDebugCanvas : MonoBehaviour
{
    [SerializeField] GameObject DebugCanvas;

    public void SwichDebugCanvas()
    {
        DebugCanvas.SetActive(!DebugCanvas.activeInHierarchy);
    }
}
