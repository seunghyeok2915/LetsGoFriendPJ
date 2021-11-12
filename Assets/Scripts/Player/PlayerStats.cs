using System;
using UnityEngine;

[Serializable]
public enum ESkill : ushort
{
    None = 0x0000,
    TwinShot = 0x0001, //Ʈ���� ����
    SideShot = 0x0002, //���̵弦
    PierceShot = 0x0004, //���뼦
    DrainHealth = 0x0008, // ����
    SplitShot = 0x0010, //���ø���
    MoveSpeedUp = 0x0020, //�̼Ӿ�
    MaxHpUp = 0x0040, //max hp ��
    AttackDelayUp = 0x0080, //���Ӿ�
    FireDotD = 0x0100, //���̾� ��Ʈ��
    Boomerang = 0x0200 ,// �θ޶�
    ExtraShuriken = 0x0400, //����Ʈ��ǥâ
    Ice = 0x0800 //���̽� ǥâ

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
    public float drainHealthPersent;

    public ushort playerSkill = 0x0000;

    public ShurikenAround extraShuriken;


    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
        playerMove = GetComponent<PlayerMove>();

        uiExpBar.SetLevel(nowLevel);
        uiExpBar.SetBar(expForLevelUp, currentExp);

        AddSkill(ESkill.DrainHealth);
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
        ushort skillByte = (ushort)skill;

        playerSkill = (ushort)(playerSkill | skillByte);

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
            case ESkill.Boomerang:
                break;
            case ESkill.ExtraShuriken:
                ExtraShuriken();
                break;
            case ESkill.DrainHealth:
                break;
            case ESkill.Ice:
                break;
            default:
                break;
        }

    }

    private void ExtraShuriken()
    {
        ShurikenAround shurikenAround = Instantiate(extraShuriken,transform.position,Quaternion.identity,transform);
        shurikenAround.SetData(playerAttack.attackDamage);
    }

    public void DrainHealth()
    {
        playerHealth.Heal(playerAttack.attackDamage * (drainHealthPersent * 0.01f));
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
        if (GameManager.Instance.IsPlaying)
        {
            // �귿 ���������
            expForLevelUp = expForLevelUp + expForLevelUp * 0.2f;
            nowLevel++;
            currentExp = 0;
            uiExpBar.SetLevel(nowLevel);
            skillSelectPanel.gameObject.SetActive(true);
        }
    }

}
