using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colorbar : MonoBehaviour
{
    public Color color_1, color_2, color_3, color_4;
    private Image image_HPgauge;
    private float timeRatio;



    // Update is called once per frame
    public void ChangeColor(CleanPlaceData data)
    {
        image_HPgauge = gameObject.GetComponent<Image>();
        timeRatio = data.FloatLastCleanPassTime()/data.FloatCleanInterval();

        if (timeRatio > 0.85f)
        {
            image_HPgauge.color = Color.Lerp(color_2, color_1, (timeRatio - 0.85f) * 4f);
        }
        else if (timeRatio > 0.50f)
        {
            image_HPgauge.color = Color.Lerp(color_3, color_2, (timeRatio - 0.50f) * 4f);
        }
        else
        {
            image_HPgauge.color = Color.Lerp(color_4, color_3, timeRatio * 4f);
        }

        image_HPgauge.fillAmount = timeRatio;
    }
}