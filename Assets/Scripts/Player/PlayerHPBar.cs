using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    public Transform player;

    public Slider hpBar;
    public GameObject HpLineFolder;
    public Text playerHpText;

    public float unitHp = 200f; // 박스 한칸당 hp

    private HorizontalLayoutGroup hpHorizontalLayoutGroup;

    // Update is called once per frame

    private void Start()
    {
        hpHorizontalLayoutGroup = HpLineFolder.GetComponent<HorizontalLayoutGroup>();
    }

    void Update()
    {
        transform.position = player.position;
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
