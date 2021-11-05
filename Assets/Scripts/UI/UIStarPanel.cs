using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStarPanel : MonoBehaviour
{
    public GameObject[] stars;
    public Text titleText;
    public Text starTimeText;

    public void Close()
    {
        for (int i = 0; i < 3; i++)
        {
            stars[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void Open(int starNum)
    {
        gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            stars[i].SetActive(false);
        }

        for (int i = 0; i < starNum; i++)
        {
            stars[i].SetActive(true);
        }
    }

    public void Open(StageVO vo)
    {
        titleText.text = $"{vo.id}스테이지 : {vo.name}";
        starTimeText.text = $"목표 클리어 시간\n{vo.star_clear_time_second.ToString("00.00")}초";
    }
}
