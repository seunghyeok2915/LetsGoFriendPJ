using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChapterInfoPanel : MonoBehaviour
{
    public Text titleText;
    public Text highStageText;

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open(int num)
    {
        highStageText.text = $"최고 스테이지 : {num}";
    }

    public void Open(ChapterVO vo)
    {
        titleText.text = $"챕터{vo.id} : {vo.name}";
        //starTimeText.text = $"목표 클리어 시간\n{vo.star_clear_time_second.ToString("00.00")}초";
    }
}
