using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TypeEffect : MonoBehaviour
{
    private Text _mText;

    private void Start()
    {
        DOTween.defaultTimeScaleIndependent = true;
        _mText = GetComponent<Text>();
        string text = _mText.text;
        DOTween.To(() => "", str => _mText.text = str, text, 2f);
    }
}
