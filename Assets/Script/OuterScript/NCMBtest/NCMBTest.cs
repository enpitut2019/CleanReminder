using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class NCMBTest : MonoBehaviour
{
    NCMBObject testClass = new NCMBObject("TestClass");

    
    // Start is called before the first frame update
    void Start()
    {
        testClass["message"] = "Hello,ddd";
        testClass.SaveAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
