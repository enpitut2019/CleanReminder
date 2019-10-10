using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DicisionButton : MonoBehaviour
{
    [SerializeField] Text text;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dicision()
    {
        Debug.Log(gameObject.name+"decision");
        if (text.text == "決定") text.text = "";
        else text.text = "決定";
    }
}
