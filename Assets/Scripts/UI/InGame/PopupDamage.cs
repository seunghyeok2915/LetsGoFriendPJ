using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupDamage : MonoBehaviour
{
    public RectTransform rectTrans;
    public Text damageText;
    public Vector3 offset;
    private Transform trans;

    public void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(trans.position + offset);
    }

    public void SetData(float damage, Transform trans)
    {
        this.trans = trans;
        damageText.color = Color.white;
        rectTrans.localScale = Vector3.one;

        damageText.text = $"-{damage}";
        rectTrans.DOScale(1.5f, 0.5f).OnComplete(() =>
        {
            rectTrans.DOScale(0, 0.5f);
            damageText.DOFade(0, 0.5f);
        });

    }
}
