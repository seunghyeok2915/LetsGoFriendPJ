using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public UISkillSlotPanel uiSkillSlotPanel;
    public UIExpBar uiExpBar;
    public int nowLevel = 1;

    public float expForLevelUp;
    public float currentExp;

    private void Start()
    {
        uiExpBar.SetLevel(nowLevel);
        uiExpBar.SetBar(expForLevelUp, currentExp);
    }

    public void AddExp(float exp)
    {
        currentExp += exp;
        if(expForLevelUp <= currentExp)
        {
            //������
            LevelUp();
        }

        uiExpBar.SetBar(expForLevelUp,currentExp);
    }

    public  void LevelUp()
    {
        // �귿 ���������
        nowLevel++;
        currentExp = 0;
        uiExpBar.SetLevel(nowLevel);
        uiSkillSlotPanel.gameObject.SetActive(true);
    }

}
