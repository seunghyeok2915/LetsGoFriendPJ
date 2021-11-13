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
        highStageText.text = $"�ְ� �������� : {num}";
    }

    public void Open(ChapterVO vo)
    {
        titleText.text = $"é��{vo.id} : {vo.name}";
        //starTimeText.text = $"��ǥ Ŭ���� �ð�\n{vo.star_clear_time_second.ToString("00.00")}��";
    }
}
