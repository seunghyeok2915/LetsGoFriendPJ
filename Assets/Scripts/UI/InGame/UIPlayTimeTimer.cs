using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayTimeTimer : MonoBehaviour
{
    public Text timeText;
    private float timeCnt;

    void Update()
    {
        timeCnt = GameManager.Instance.PlayTime;
        string timeStr;
        timeStr = $"{timeCnt.ToString("00.00")}√ ";
        timeStr = timeStr.Replace(".", ":");
        timeText.text = "Time : " + timeStr;
    }
}
