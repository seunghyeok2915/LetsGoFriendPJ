using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopPage : MonoBehaviour
{
    public RectTransform rectTransform;
    public LobbyManager lobbyManager;
    public Button closeBtn;

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


    public void OnEnable()
    {
        rectTransform.anchoredPosition = new Vector2(-2500, -458);
        rectTransform.DOAnchorPosX(0, 1f);
    }

    public void Start()
    {
        closeBtn.onClick.AddListener(() => rectTransform.DOAnchorPosX(-2500, 1f).OnComplete(() => gameObject.SetActive(false)));

        attackSpeedBtn.onClick.AddListener(OnClickAttackSpeedBtn);
        maxHpBtn.onClick.AddListener(OnClickMaxHpBtnBtn);
        inComeBtn.onClick.AddListener(OnClickInComeLevelBtn);

        if (PlayerPrefs.HasKey("attackSpeedLevel"))
        {
            attackSpeedLevel = PlayerPrefs.GetInt("attackSpeedLevel");
        }
        else
        {
            PlayerPrefs.SetInt("attackSpeedLevel", 0);
        }

        if (PlayerPrefs.HasKey("maxHpLevel"))
        {
            maxHpLevel = PlayerPrefs.GetInt("maxHpLevel");
        }
        else
        {
            PlayerPrefs.SetInt("maxHpLevel", 0);
        }

        if (PlayerPrefs.HasKey("inComeLevel"))
        {
            inComeLevel = PlayerPrefs.GetInt("inComeLevel");
        }
        else
        {
            PlayerPrefs.SetInt("inComeLevel", 0);
        }

        SetAttackSpeedCost();
        SetMaxHpCost();
        SetInComeCost();
    }

    private void SetAttackSpeedCost()
    {
        attackSpeedCost = 100 + 50 * attackSpeedLevel;
        attackSpeedLevelText.text = $"LV{attackSpeedLevel + 1} 공격속도";
        attackSpeedCostText.text = attackSpeedCost.ToString();

    }
    private void SetMaxHpCost()
    {
        maxHpCost = 100 + 50 * maxHpLevel;
        maxHpLevelText.text = $"LV{maxHpLevel + 1} 최대 체력";
        maxHpCostText.text = maxHpCost.ToString();
    }

    private void SetInComeCost()
    {
        inComeCost = 100 + 50 * inComeLevel;
        inComeLevelText.text = $"LV{inComeLevel + 1} 수익";
        inComeCostText.text = inComeCost.ToString();

    }


    private void OnClickAttackSpeedBtn()
    {
        if(lobbyManager.CanUseZem(attackSpeedCost))
        {
            attackSpeedLevel++;
            PlayerPrefs.SetInt("attackSpeedLevel", attackSpeedLevel);
            SetAttackSpeedCost();
            //코스트 재설정 해줘야해
        }
        else
        {
            attackSpeedBtn.interactable = false;
        }
    }

    private void OnClickMaxHpBtnBtn()
    {
        if (lobbyManager.CanUseZem(maxHpCost))
        {
            maxHpLevel++;
            PlayerPrefs.SetInt("maxHpLevel", maxHpLevel);
            SetMaxHpCost();
            //코스트 재설정 해줘야해
        }
        else
        {
            maxHpBtn.interactable=false;
        }
    }

    private void OnClickInComeLevelBtn()
    {
        if (lobbyManager.CanUseZem(inComeCost))
        {
            inComeLevel++;
            PlayerPrefs.SetInt("inComeLevel", inComeLevel);
            SetInComeCost();
            //코스트 재설정 해줘야해
        }
        else
        {
            inComeBtn.interactable = false;
        }
    }


}
