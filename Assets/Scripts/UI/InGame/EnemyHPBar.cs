using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public Vector3 offset;

    public Image hpBar;
    public Image hpBackBar;

    private Transform targetTrm;

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(targetTrm.position + offset);
    }

    public void Init(Transform target)
    {
        targetTrm = target;
    }

    public void SetHPBar(float maxHp, float currentHp)
    {
        gameObject.SetActive(true);

        float fillAmount = currentHp / maxHp;
        if (fillAmount == 0)
            gameObject.SetActive(false);

        DOTween.To(() => hpBar.fillAmount, x => hpBar.fillAmount = x, fillAmount, 0.3f).OnComplete(() =>
        {
            DOTween.To(() => hpBackBar.fillAmount, x => hpBackBar.fillAmount = x, fillAmount, 0.8f);
        });
    }
}
