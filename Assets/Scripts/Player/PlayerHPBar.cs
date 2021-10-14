using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    public Vector3 offset;

    public Slider hpBar;
    public GameObject HpLineFolder;
    public Text playerHpText;

    public float unitHp; // 박스 한칸당 hp

    private Transform playerTrm;
    private HorizontalLayoutGroup hpHorizontalLayoutGroup;

    void Update()
    {
        transform.position = playerTrm.position + offset; // 플레이어 따라다니는 스크립트
    }

    public void Init(Transform player)
    {
        playerTrm = player;
    }

    public void SetHPBar(float maxHp, float currentHp)
    {
        hpBar.value = currentHp / maxHp;
        playerHpText.text = "" + currentHp;

        float scaleX = (5 * unitHp) / (maxHp);

        if (hpHorizontalLayoutGroup == null)
            hpHorizontalLayoutGroup = HpLineFolder.GetComponent<HorizontalLayoutGroup>();

        hpHorizontalLayoutGroup.gameObject.SetActive(false);

        Vector3 newScale = new Vector3(scaleX, 1, 1);
        foreach (Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = newScale;
        }

        hpHorizontalLayoutGroup.gameObject.SetActive(true);
    }
}
