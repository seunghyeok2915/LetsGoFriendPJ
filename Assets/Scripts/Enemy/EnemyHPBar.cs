using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHPBar : MonoBehaviour
{
    public Vector3 offset;

    public Image hpBar;
    public Image hpBackBar;

    private Transform targetTrm;

    void Update()
    {
        transform.position = targetTrm.position + offset; // 플레이어 따라다니는 스크립트
    }

    public void Init(Transform player)
    {
        targetTrm = player;
    }

    public void SetHPBar(float maxHp, float currentHp)
    {
        DOTween.To(() => hpBar.fillAmount, x => hpBar.fillAmount = x, currentHp / maxHp, 0.3f).OnComplete(() =>
        {
            DOTween.To(() => hpBackBar.fillAmount, x => hpBackBar.fillAmount = x, currentHp / maxHp, 0.8f);
        });
    }
}
