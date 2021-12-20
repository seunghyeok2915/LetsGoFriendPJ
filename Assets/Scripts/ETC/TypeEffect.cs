using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TypeEffect : MonoBehaviour
{
    private Text _mText;

    private string text = null;

    private void Start()
    {
        _mText = GetComponent<Text>();
        text = _mText.text;

        DOTween.defaultTimeScaleIndependent = true;

        _mText.text = "";
        DOTween.To(() => "", str => _mText.text = str, text, 2f);
    }
}
