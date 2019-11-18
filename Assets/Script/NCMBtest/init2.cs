using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init2 : MonoBehaviour
{
    [SerializeField] GameObject[] inits;

    public void InitActive()
    {
        foreach(var obj in inits)
        {
            obj.SetActive(false);
        }
    }
}
