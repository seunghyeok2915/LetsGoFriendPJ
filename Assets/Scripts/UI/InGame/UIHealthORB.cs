using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHealthORB : MonoBehaviour
{
    public Image hpImage;
    public Text hpText;

    private Transform targetTrm;

    private float fillAmount;
    private bool canUpdate = false;

    private void Update()
    {
        if (canUpdate)
            hpImage.material.SetFloat("OffsetUV_Y_1", 1 - fillAmount);
    }

    public void SetHPBar(float maxHp, float currentHp)
    {
        var nowHp = currentHp / maxHp;
        if (nowHp <= 0)
        {
            hpText.text = "Dead";
        }
        else
        {
            hpText.text = currentHp.ToString();
        }

        fillAmount = 1 - hpImage.material.GetFloat("OffsetUV_Y_1");

        canUpdate = true;
        DOTween.To(() => fillAmount, x => fillAmount = x, nowHp, 0.5f).OnComplete(() => canUpdate = false);

    }
}
