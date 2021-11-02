using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UISkillSelectPanel : MonoBehaviour
{
    public Image slotThemeImage;
    public Text slotThemeName;
    //�ʿ��Ѱ͵�
    public SlotTheme[] slotThemes;

    //��ų ���� ����
    public Button[] buttons;

    private void Start()
    {
        RegisterButtons();
        ShowSlotTheme(SelectRandomTheme());
    }

    public void ShowSlotTheme(int num)
    {
        slotThemeImage.sprite = slotThemes[num].slotImage;
        slotThemeName.text = slotThemes[num].slotName;
    }

    private int SelectRandomTheme()
    {
        return UnityEngine.Random.Range(0, slotThemes.Length);
    }

    private void RegisterButtons()
    {
        for (int i = 0; i < buttons.Length; i++) //Register Button
        {
            buttons[i].onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {

    }


}
