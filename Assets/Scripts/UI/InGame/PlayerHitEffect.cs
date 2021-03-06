using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitEffect : MonoBehaviour
{
    public float fadeInTime;
    public float fadeOutTime;

    private Image hitImage;

    private Sequence sq;

    private void Start()
    {
        hitImage = GetComponent<Image>();
        sq = DOTween.Sequence()
            .Append(hitImage.DOFade(0.50f, fadeInTime))
            .Append(hitImage.DOFade(0, fadeOutTime))
            .OnStart(() => hitImage.color = new Color(1, 1, 1, 0))
            .SetAutoKill(false)
            .Pause()
            .OnComplete(() => gameObject.SetActive(false));
    }

    public void OnHitEffect()
    {
        SoundManager.Instance.PlayFXSound("onhit");
        gameObject.SetActive(true);
        sq.Restart();
    }
}
