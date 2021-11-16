using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurrentStage : MonoBehaviour
{
    public Text currentStageText;
    public void UpdateText(int stage)
    {
        currentStageText.text = $"현제 스테이지 = {stage + 1}";
    }
}
