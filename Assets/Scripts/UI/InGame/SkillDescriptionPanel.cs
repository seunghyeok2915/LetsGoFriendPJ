using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDescriptionPanel : MonoBehaviour
{
    public Image icon;
    public Text nameText;
    public Text descriptionText;

    public Button closeBtn;

    public void Start()
    {
        closeBtn.onClick.AddListener(Close);
    }
    public void Open(SlotSkill skill)
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
        icon.sprite = skill.skillImage;
        nameText.text = skill.skillName;
        descriptionText.text = skill.skillDefinition;
    }

    public void Close()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
