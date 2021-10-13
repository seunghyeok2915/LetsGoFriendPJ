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

    public float test;

    public float maxHp;
    public float currentHp;
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
        SetHPBar();
    }

    public void SetHPBar()
    {
        hpBar.value = currentHp / maxHp;
        playerHpText.text = "" + currentHp;

        float scaleX = (5 * unitHp) / (maxHp);
        Debug.Log(scaleX);

        hpHorizontalLayoutGroup.gameObject.SetActive(false);

        Vector3 newScale = new Vector3(scaleX, 1, 1);
        foreach (Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = newScale;
        }

        hpHorizontalLayoutGroup.gameObject.SetActive(true);
    }
}
