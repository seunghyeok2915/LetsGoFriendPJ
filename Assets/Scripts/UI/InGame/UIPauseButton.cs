using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseButton : MonoBehaviour
{
    private Button _button;
    public UIPausePanel uiPausePanel;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(uiPausePanel.Open);
    }
}
