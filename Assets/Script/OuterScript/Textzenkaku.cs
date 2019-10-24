using UnityEngine;
using UnityEngine.UI;

public class Textzenkaku : MonoBehaviour
{
    InputField T1;
    string S = "";
    int pos = 0;
    int Tpos = 0;
    // Use this for initialization
    void Start()
    {
        T1 = gameObject.GetComponent<InputField>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (T1.text != "")
            {
                if (S != T1.text)
                {
                    if (S == "")
                    {
                        if (T1.text.Length > 1 && T1.text.Length % 2 == 0)
                        {
                            int cun = T1.text.Length;
                            if (T1.text.Substring(0, cun / 2) == T1.text.Substring(cun / 2, cun / 2))
                            {
                                T1.text = T1.text.Substring(0, cun / 2);
                            }
                        }
                    }
                    else
                    {
                        if (T1.text.Length - Tpos > 1)
                        {

                            if (Tpos + (pos - Tpos) * 2 == T1.text.Length)
                            {

                                string hantei = T1.text.Substring(Tpos);

                                if (hantei.Length > 1 && hantei.Length % 2 == 0)
                                {
                                    int cun = hantei.Length;
                                    if (hantei.Substring(0, cun / 2) == hantei.Substring(cun / 2, cun / 2))
                                    {

                                        T1.text = T1.text.Substring(0, Tpos) + hantei.Substring(0, cun / 2);
                                    }
                                }
                            }
                            else
                            {
                                Debug.Log((T1.text.Length - (Tpos + (pos - Tpos) * 2)) / 2);
                                int usiro = (T1.text.Length - (Tpos + (pos - Tpos) * 2)) / 2;
                                int mae = Tpos - usiro;
                                string hantei = T1.text.Remove(T1.text.Length - usiro).Substring(mae);
                                if (hantei.Length > 1 && hantei.Length % 2 == 0)
                                {
                                    int cun = hantei.Length;
                                    if (hantei.Substring(0, cun / 2) == hantei.Substring(cun / 2, cun / 2))
                                    {

                                        T1.text = T1.text.Substring(0, mae) + hantei.Substring(0, cun / 2) + T1.text.Substring(T1.text.Length - usiro, usiro);
                                    }
                                }
                            }
                        }

                    }

                }
            }
        }
        S = T1.text;
        Tpos = T1.text.Length;
        pos = T1.selectionAnchorPosition;

    }
}