using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExpBar : MonoBehaviour
{
    public Text levelText;
    public Image fillImage;

    public void SetLevel(int level)
    {
        levelText.text = $"Lv.{level}";
    }

    public void SetBar(float max,float cur)
    {
        fillImage.fillAmount = Mathf.Clamp01(cur / max);
    }
}
