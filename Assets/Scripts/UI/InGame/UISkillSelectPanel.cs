using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class UISkillSelectPanel : MonoBehaviour
{
    public float closeDelay = 2f;
    public bool isPressed;

    public Image slotThemeImage; //주제 이미지
    public Text slotThemeName; //주제 텍스트

    public Text skillDescription;
    private string originDescription;

    public SlotTheme[] slotThemes; //스킬 주제

    public Button[] buttons; //버튼들 

    public int selectedSlotNum; //선택한 주제 넘버

    private Sprite originButtonSprite;

    private void Start()
    {
        RegisterButtons();
        originButtonSprite = buttons[0].image.sprite;
        originDescription = skillDescription.text;
    }

    private void OnEnable()
    {
        selectedSlotNum = SelectRandomTheme();
        ShowSlotTheme(selectedSlotNum);
        Time.timeScale = 0.0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        ResetPanel();
    }

    public SlotSkill SelectRandomSkillOfTheme(int num)
    {
        return slotThemes[num].slotSkills[UnityEngine.Random.Range(0, slotThemes[num].slotSkills.Length)];
    }

    public void ShowSlotTheme(int num)
    {
        slotThemeImage.sprite = slotThemes[num].slotImage;
        slotThemeName.text = slotThemes[num].slotName;

        SelectRandomSkillOfTheme(num);
    }

    private int SelectRandomTheme()
    {
        return UnityEngine.Random.Range(0, slotThemes.Length);
    }

    private void ResetPanel()
    {
        for (int i = 0; i < buttons.Length; i++) //Register Button
        {
            buttons[i].image.sprite = originButtonSprite;
            buttons[i].interactable = true;
        }

        isPressed = false;
        skillDescription.text = originDescription;
    }

    private void RegisterButtons()
    {
        for (int i = 0; i < buttons.Length; i++) //Register Button
        {
            int cnt = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(cnt));
        }
    }

    private void OnButtonClick(int num)
    {
        if (isPressed)
        {
            return;
        }

        isPressed = true;

        SlotSkill skill = SelectRandomSkillOfTheme(selectedSlotNum);

        string definition = skill.skillName + "\n\n" + skill.skillDefinition;
        buttons[num].image.sprite = skill.skillImage;
        skillDescription.text = definition;
        GameManager.Instance.GetPlayer().GetComponent<PlayerStats>().AddSkill(skill.skillEnum);

        for (int i = 0; i < buttons.Length; i++) //Register Button
        {
            if (i != num)
            {
                buttons[i].interactable = false;
            }
        }

        StartCoroutine(ClosePanel(closeDelay));
    }
    private IEnumerator ClosePanel(float second)
    {
        yield return new WaitForSecondsRealtime(second);
        gameObject.SetActive(false);
    }
}

