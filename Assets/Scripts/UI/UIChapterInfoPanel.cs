using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChapterInfoPanel : MonoBehaviour
{
    public Text titleText;
    public Text titleBackText;

    public Text highStageText;
    public Text highStageBackText;

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open(int num)
    {
        string content = $"{num} ��������";
        highStageText.text = content;
        highStageBackText.text = content;
    }

    public void Open(ChapterVO vo)
    {
        string content = $"Chapter {vo.id} : {vo.name}";
        titleText.text = content;
        titleBackText.text = content;
        //starTimeText.text = $"��ǥ Ŭ���� �ð�\n{vo.star_clear_time_second.ToString("00.00")}��";
    }
}
