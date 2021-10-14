using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHealthORB : MonoBehaviour
{
    public Image hpBar;
    public Image hpBackBar;
    public Text hpText;

    private Transform targetTrm;

    public void SetHPBar(float maxHp, float currentHp)
    {
        var nowHp = currentHp / maxHp;
        hpText.text = currentHp.ToString();
        DOTween.To(() => hpBar.fillAmount, x => hpBar.fillAmount = x, nowHp, 0.3f).OnComplete(() =>
        {
            DOTween.To(() => hpBackBar.fillAmount, x => hpBackBar.fillAmount = x, currentHp / maxHp, 0.8f);
        });
    }
}
