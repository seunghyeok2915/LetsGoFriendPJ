using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISkillSelectPanel : MonoBehaviour
{
    public UIStatsPanel uiStatsPanel;

    public float closeDelay = 2f; // 닫히는 시간
    public bool isPressed; //이미 버튼을 눌렀는가

    public Image slotThemeImage; //주제 이미지
    public Text slotThemeName; //주제 텍스트

    public Text skillDescription; //스킬 설명
    private string originDescription; //원래 설명

    public SlotDeck slotDeck;
    private List<SlotTheme> slotThemes = new List<SlotTheme>();

    public Button[] buttons; //버튼들 

    public int selectedSlotNum; //선택한 주제 넘버

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
            //다쓴것임
            print("Theme 도없음");
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
            //다쓴것임
            Time.timeScale = 1;
            print("Theme 도없음");
            gameObject.SetActive(false);
            return;
        }

        selectedSlotNum = SelectRandomTheme();
        ShowSlotTheme(selectedSlotNum);

        if (!CheckTheme(selectedSlotNum))
        {
            print("지움" + slotThemes[selectedSlotNum]);
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
        if (slotThemes[num].slotSkills.Count <= 0) //없을땐 null을 보냄
        {
            print("Theme 에 스킬이 없음");
            return false;
        }
        return true;
    }

    private SlotSkill SelectRandomSkillOfTheme(int num)
    {
        int randnum = UnityEngine.Random.Range(0, slotThemes[num].slotSkills.Count);
        SlotSkill slotskill = slotThemes[num].slotSkills[randnum];

        slotThemes[num].slotSkills.Remove(slotskill);
        print("지움" + slotskill.name);

        if(slotThemes[num].slotName == "표창 능력")
        {
            print("표창능력은 한번만");
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

