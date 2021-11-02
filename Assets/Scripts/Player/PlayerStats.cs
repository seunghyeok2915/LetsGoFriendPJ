using System;
using UnityEngine;

[Serializable]
public enum ESkill : byte
{
    None = 0x0000,
    TwinShot = 0x0001,
    SideShot = 0x0002,
    PierceShot = 0x0004,
    BounceShot = 0x0008, // 안씀
    SplitShot = 0x0010
}

public class PlayerStats : MonoBehaviour
{
    public UISkillSlotPanel uiSkillSlotPanel;
    public UIExpBar uiExpBar;
    public int nowLevel = 1;

    public float expForLevelUp;
    public float currentExp;

    public byte playerSkill = 0x0000;

    private void Start()
    {
        uiExpBar.SetLevel(nowLevel);
        uiExpBar.SetBar(expForLevelUp, currentExp);

        //AddSkill(ESkill.PierceShot);
        AddSkill(ESkill.TwinShot);
        AddSkill(ESkill.SplitShot);
        AddSkill(ESkill.SideShot);
        AddSkill(ESkill.BounceShot);
    }

    public bool CanUseSkill(ESkill skill) //스킬이 사용가능한지
    {
        byte skillByte = (byte)skill;

        if ((playerSkill & skillByte) == skillByte)
        {
            return true;
        }
        
        return false;
    }

    public void AddSkill(ESkill skill) //스킬 추가
    {
        byte skillByte = (byte)skill;

        playerSkill = (byte)(playerSkill | skillByte);

    }

    public void AddExp(float exp)
    {
        currentExp += exp;
        if (expForLevelUp <= currentExp)
        {
            //레벨업
            LevelUp();
        }

        uiExpBar.SetBar(expForLevelUp, currentExp);
    }

    public void LevelUp()
    {
        // 룰렛 띄워여ㅑ함
        nowLevel++;
        currentExp = 0;
        uiExpBar.SetLevel(nowLevel);
        uiSkillSlotPanel.gameObject.SetActive(true);
    }

}
