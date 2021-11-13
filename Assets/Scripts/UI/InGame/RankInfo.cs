using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RankInfo : MonoBehaviour
{
    public Text nameRank;
    public Text starScore;

    public void SetData(int rank, RecordVO vo)
    {
        nameRank.text = $"{rank.ToString()}위 : {vo.name}";
        starScore.text = $"{vo.total_stage} 스테이지";
    }

    public void SetAnimation(float delay)
    {
        transform.localScale = new Vector3(1, 0, 1);
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delay);
        seq.Append(transform.DOScaleY(1, 0.4f));
    }
}
