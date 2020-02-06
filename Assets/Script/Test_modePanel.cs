using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test_modePanel : MonoBehaviour
{
    [SerializeField] Test_modePanel beforePanel;
    [SerializeField] Test_modePanel[] nextPanel;
    [SerializeField] string[] key;

    [SerializeField] UnityEvent awakeAction;
    [SerializeField] UnityEvent closeAction;
}
