using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    public GameObject obj;
    DicisionButton db;
    // Start is called before the first frame update
    void Start()
    {
        db = obj.GetComponent<DicisionButton>();
    }

    // Update is called once per frame
    void Update()
    {

        db.Dicision();
    }
}
