using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPage : MonoBehaviour
{
    public LobbyManager lobbyManager;

    public Button attackSpeedBtn;
    public Button maxHpBtn;
    public Button inComeBtn;

    public Text attackSpeedLevelText;
    public Text maxHpLevelText;
    public Text inComeLevelText;

    public Text attackSpeedCostText;
    public Text maxHpCostText;
    public Text inComeCostText;

    private int attackSpeedCost;
    private int maxHpCost;
    private int inComeCost;

    private int attackSpeedLevel;
    private int maxHpLevel;
    private int inComeLevel;

    public void Start()
    {
        attackSpeedBtn.onClick.AddListener(OnClickAttackSpeedBtn);
        maxHpBtn.onClick.AddListener(OnClickMaxHpBtnBtn);
        inComeBtn.onClick.AddListener(OnClickInComeLevelBtn);

        attackSpeedLevel = PlayerPrefs.GetInt("attackSpeedLevel");
        maxHpLevel = PlayerPrefs.GetInt("maxHpLevel");
        inComeLevel = PlayerPrefs.GetInt("inComeLevel");

        SetAttackSpeedCost();
        SetMaxHpCost();
        SetInComeCost();
    }

    private void SetAttackSpeedCost()
    {
        attackSpeedCost = 100 + 50 * attackSpeedLevel;
        attackSpeedLevelText.text = $"LV{attackSpeedLevel + 1} ���ݼӵ�";
        attackSpeedCostText.text = attackSpeedCost.ToString();
    }
    private void SetMaxHpCost()
    {
        maxHpCost = 100 + 50 * maxHpLevel;
        maxHpLevelText.text = $"LV{maxHpLevel + 1} �ִ� ü��";
        maxHpCostText.text = maxHpCost.ToString();
    }

    private void SetInComeCost()
    {
        inComeCost = 100 + 50 * inComeLevel;
        inComeLevelText.text = $"LV{inComeLevel + 1} ����";
        inComeCostText.text = inComeCost.ToString();
    }


    private void OnClickAttackSpeedBtn()
    {
        if(lobbyManager.CanUseZem(attackSpeedCost))
        {
            attackSpeedLevel++;
            PlayerPrefs.SetInt("attackSpeedLevel", attackSpeedLevel);
            SetAttackSpeedCost();
            //�ڽ�Ʈ �缳�� �������
        }
    }

    private void OnClickMaxHpBtnBtn()
    {
        if (lobbyManager.CanUseZem(maxHpCost))
        {
            maxHpLevel++;
            PlayerPrefs.SetInt("maxHpLevel", maxHpLevel);
            SetMaxHpCost();
            //�ڽ�Ʈ �缳�� �������
        }
    }

    private void OnClickInComeLevelBtn()
    {
        if (lobbyManager.CanUseZem(inComeCost))
        {
            inComeLevel++;
            PlayerPrefs.SetInt("inComeLevel", inComeLevel);
            SetInComeCost();
            //�ڽ�Ʈ �缳�� �������
        }
    }


}
