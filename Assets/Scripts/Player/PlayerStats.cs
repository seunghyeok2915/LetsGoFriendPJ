using System;
using UnityEngine;

[Serializable]
public enum ESkill : short
{
    None = 0x0000,
    TwinShot = 0x0001, //Ʈ���� ����
    SideShot = 0x0002, //���̵弦
    PierceShot = 0x0004, //���뼦
    BounceShot = 0x0008, // �Ⱦ�
    SplitShot = 0x0010, //���ø���
    MoveSpeedUp = 0x0020, //�̼Ӿ�
    MaxHpUp = 0x0040, //max hp ��
    AttackDelayUp = 0x0080, //���Ӿ�
    FireDotD = 0x0100 //���̾� ��Ʈ��
}

public class PlayerStats : MonoBehaviour
{
    public UISkillSelectPanel skillSelectPanel;

    public UIExpBar uiExpBar;
    public UIEarnZem uiEarnZem;

    public PlayerHealth playerHealth;
    public PlayerAttack playerAttack;
    public PlayerMove playerMove;
    public int nowLevel = 1;

    public float expForLevelUp;
    public float currentExp;

    public short playerSkill = 0x0000;


    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
        playerMove = GetComponent<PlayerMove>();

        uiExpBar.SetLevel(nowLevel);
        uiExpBar.SetBar(expForLevelUp, currentExp);
    }

    public bool CanUseSkill(ESkill skill) //��ų�� ��밡������
    {
        short skillByte = (short)skill;

        if ((playerSkill & skillByte) == skillByte)
        {
            return true;
        }

        return false;
    }

    public void AddSkill(ESkill skill) //��ų �߰�
    {
        short skillByte = (short)skill;

        playerSkill = (short)(playerSkill | skillByte);

        switch (skill)
        {
            case ESkill.None:
                break;
            case ESkill.TwinShot:
                break;
            case ESkill.SideShot:
                break;
            case ESkill.PierceShot:
                break;
            case ESkill.BounceShot:
                break;
            case ESkill.SplitShot:
                break;
            case ESkill.MoveSpeedUp:
                playerMove.IncreaseMoveSpeed(5);
                break;
            case ESkill.MaxHpUp:
                playerHealth.IncreaseMaxHp(100);
                break;
            case ESkill.AttackDelayUp:
                playerAttack.AttackDelayUp(0.1f);
                break;
            case ESkill.FireDotD:
                break;
            default:
                break;
        }

    }

    public void AddExp(float exp)
    {
        currentExp += exp;
        if (expForLevelUp <= currentExp)
        {
            //������
            LevelUp();
        }

        uiExpBar.SetBar(expForLevelUp, currentExp);
        int zemCnt = GameManager.Instance.EarnZem += (int)exp;
        uiEarnZem.UpdateZem(zemCnt);
    }

    public void LevelUp()
    {
        // �귿 ���������
        expForLevelUp = expForLevelUp + expForLevelUp * 0.2f;
        nowLevel++;
        currentExp = 0;
        uiExpBar.SetLevel(nowLevel);
        skillSelectPanel.gameObject.SetActive(true);
    }

}
