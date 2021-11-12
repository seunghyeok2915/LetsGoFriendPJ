using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISkillSelectPanel : MonoBehaviour
{
    public UIStatsPanel uiStatsPanel;

    public float closeDelay = 2f; // ������ �ð�
    public bool isPressed; //�̹� ��ư�� �����°�

    public Image slotThemeImage; //���� �̹���
    public Text slotThemeName; //���� �ؽ�Ʈ

    public Text skillDescription; //��ų ����
    private string originDescription; //���� ����

    public SlotDeck slotDeck;
    private List<SlotTheme> slotThemes = new List<SlotTheme>();

    public Button[] buttons; //��ư�� 

    public int selectedSlotNum; //������ ���� �ѹ�

    private Sprite originButtonSprite;
    private bool isFirst = true;

    private void Start()
    {
        RegisterButtons();

        originButtonSprite = buttons[0].image.sprite;
        originDescription = skillDescription.text;
    }

    private void RegisterSlotThemes()
    {
        for (int i = 0; i < slotDeck.themes.Count; i++)
        {
            SlotTheme newTheme = ScriptableObject.CreateInstance<SlotTheme>();

            newTheme.slotName = slotDeck.themes[i].slotName;
            newTheme.slotImage = slotDeck.themes[i].slotImage;
            newTheme.slotSkills = new List<SlotSkill>();


            for (int j = 0; j < slotDeck.themes[i].slotSkills.Count; j++)
            {
                SlotSkill newSkill = ScriptableObject.CreateInstance<SlotSkill>();

                newSkill.skillDefinition = slotDeck.themes[i].slotSkills[j].skillDefinition;
                newSkill.skillEnum = slotDeck.themes[i].slotSkills[j].skillEnum;
                newSkill.skillImage = slotDeck.themes[i].slotSkills[j].skillImage;
                newSkill.skillName = slotDeck.themes[i].slotSkills[j].skillName;

                newTheme.slotSkills.Add(newSkill);
            }


            slotThemes.Add(newTheme);
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
        if (slotThemes.Count <= 0 && isFirst)
        {
            isFirst = false;
            RegisterSlotThemes();
        }

        if(slotThemes.Count <= 0 && !isFirst)
        {
            //�پ�����
            print("Theme ������");
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            return;
        }

        PickRandom();
    }

    private void PickRandom()
    {
        if (slotThemes.Count <= 0 && !isFirst)
        {
            //�پ�����
            Time.timeScale = 1;
            print("Theme ������");
            gameObject.SetActive(false);
            return;
        }

        selectedSlotNum = SelectRandomTheme();
        ShowSlotTheme(selectedSlotNum);

        if (!CheckTheme(selectedSlotNum))
        {
            print("����" + slotThemes[selectedSlotNum]);
            slotThemes.Remove(slotThemes[selectedSlotNum]);
            PickRandom();
            return;
        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        ResetPanel();
    }

    private bool CheckTheme(int num)
    {
        if (slotThemes[num].slotSkills.Count <= 0) //������ null�� ����
        {
            print("Theme �� ��ų�� ����");
            return false;
        }
        return true;
    }

    private SlotSkill SelectRandomSkillOfTheme(int num)
    {
        int randnum = UnityEngine.Random.Range(0, slotThemes[num].slotSkills.Count);
        SlotSkill slotskill = slotThemes[num].slotSkills[randnum];

        slotThemes[num].slotSkills.Remove(slotskill);
        print("����" + slotskill.name);

        if(slotThemes[num].slotName == "ǥâ �ɷ�")
        {
            print("ǥâ�ɷ��� �ѹ���");
            slotThemes.Remove(slotThemes[num]);
        }

        return slotskill;
    }

    public void ShowSlotTheme(int num)
    {
        slotThemeImage.sprite = slotThemes[num].slotImage;
        slotThemeName.text = slotThemes[num].slotName;
    }

    private int SelectRandomTheme()
    {
        int randNum = UnityEngine.Random.Range(0, slotThemes.Count);
        print(randNum + "random");
        return randNum;
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

        if (skill != null)
        {
            string definition = skill.skillName + "\n\n" + skill.skillDefinition;
            buttons[num].image.sprite = skill.skillImage;
            skillDescription.text = definition;
            GameManager.Instance.GetPlayer().GetComponent<PlayerStats>().AddSkill(skill.skillEnum);
            uiStatsPanel.AddSkill(skill);
        }


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

