using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISkillSelectPanel : MonoBehaviour
{
    public UIStatsPanel uiStatsPanel;
    public Image backImage;

    public Text closeTimeText;
    public Button skillBuyButton;

    public Button skipButton; //끔
    public Text buyCostText;
    public int buyCost = 1000;


    public float closeDelay = 2f; // 닫히는 시간
    public bool isPressed; //이미 버튼을 눌렀는가

    public Image slotThemeImage; //주제 이미지
    public Text slotThemeName; //주제 텍스트

    public Text skillDescription; //스킬 설명
    private string originDescription; //원래 설명

    public HorizontalLayoutGroup chestsGroup;

    public SlotDeck slotDeck;
    private List<SlotTheme> slotThemes = new List<SlotTheme>();

    public Button[] buttons; //버튼들 

    public int selectedSlotNum; //선택한 주제 넘버

    private Sprite originButtonSprite;
    private bool isFirst = true;
    private bool buyUsed = false;

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

    private void OnOpenAnim()
    {
        DOTween.defaultTimeScaleIndependent = true;
        backImage.color = new Color(0, 0, 0, 0);
        backImage.DOFade(0.9f, 0.5f);

        slotThemeImage.rectTransform.anchoredPosition = new Vector3(0, 400, 0);
        slotThemeImage.rectTransform.DOLocalMove(new Vector3(0, 925, 0), 0.5f);
        slotThemeImage.DOFade(0, 0.01f);
        slotThemeImage.DOFade(0.9f, 0.5f);

        skillDescription.rectTransform.anchoredPosition = new Vector3(0, -1365, 0);
        skillDescription.rectTransform.DOLocalMove(new Vector3(0, -747, 0), 0.7f);
        skillDescription.DOFade(0, 0.01f);
        skillDescription.DOFade(0.9f, 0.7f);

        chestsGroup.spacing = -2000f;
        DOTween.To(() => chestsGroup.spacing, x => chestsGroup.spacing = x, 0, 1f);
    }

    private void OnEnable()
    {
        Time.timeScale = 0.0f;

        OnOpenAnim();

        if (slotThemes.Count <= 0 && isFirst) //처음 킨거라면
        {
            isFirst = false;
            RegisterSlotThemes();
        }

        if (slotThemes.Count <= 0 && !isFirst)
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
        if (slotThemes.Count <= 0 && !isFirst) //(!isFirst || buyUsed)
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
        buyUsed = false;
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

        if (slotThemes[num].slotName == "표창 능력")
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

        skipButton.gameObject.SetActive(false);
        //skillBuyButton.gameObject.SetActive(false);

        sequence.Kill();
    }

    private void RegisterButtons()
    {
        for (int i = 0; i < buttons.Length; i++) //Register Button
        {
            int cnt = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(cnt));
        }

        skipButton.onClick.AddListener(Close);
        skipButton.gameObject.SetActive(false);

        //buyCostText.text = buyCost.ToString();

        //skillBuyButton.onClick.RemoveAllListeners();
        //skillBuyButton.onClick.AddListener(OnClickBuySkillButton); //buy 스킬 버튼 이벤트 넣고 false
        //skillBuyButton.gameObject.SetActive(false);
    }

    private void OnClickBuySkillButton() //구매 버튼
    {

        if (GameManager.Instance.UseEarnZem(buyCost))
        {
            StopAllCoroutines();
            PickRandom();
            OnOpenAnim();
            ResetPanel();
            buyUsed = true;
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private Sequence sequence;

    private void OnButtonClick(int num) //버튼 눌렀을때
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

            sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(() => "", str => skillDescription.text = str, definition, 1.5f));
            //skillDescription.text = definition;
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

        skipButton.gameObject.SetActive(true);
        //if (!buyUsed)
        //{
        //    skillBuyButton.gameObject.SetActive(true);
        //}

        StartCoroutine(ClosePanel(closeDelay));
    }
    private IEnumerator ClosePanel(float second)
    {

        while (true)
        {
            closeTimeText.text = $"게임 시작까지 {second}초";
            if (second <= 0)
            {
                break;
            }

            second -= 1;
            yield return new WaitForSecondsRealtime(1f);
        }

        Close();
    }
}

