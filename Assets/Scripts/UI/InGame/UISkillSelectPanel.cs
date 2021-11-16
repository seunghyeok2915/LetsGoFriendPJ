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

    public Button skipButton; //��
    public Text buyCostText;
    public int buyCost = 1000;


    public float closeDelay = 2f; // ������ �ð�
    public bool isPressed; //�̹� ��ư�� �����°�

    public Image slotThemeImage; //���� �̹���
    public Text slotThemeName; //���� �ؽ�Ʈ

    public Text skillDescription; //��ų ����
    private string originDescription; //���� ����

    public HorizontalLayoutGroup chestsGroup;

    public SlotDeck slotDeck;
    private List<SlotTheme> slotThemes = new List<SlotTheme>();

    public Button[] buttons; //��ư�� 

    public int selectedSlotNum; //������ ���� �ѹ�

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

        if (slotThemes.Count <= 0 && isFirst) //ó�� Ų�Ŷ��
        {
            isFirst = false;
            RegisterSlotThemes();
        }

        if (slotThemes.Count <= 0 && !isFirst)
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
        if (slotThemes.Count <= 0 && !isFirst) //(!isFirst || buyUsed)
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
        buyUsed = false;
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

        if (slotThemes[num].slotName == "ǥâ �ɷ�")
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
        //skillBuyButton.onClick.AddListener(OnClickBuySkillButton); //buy ��ų ��ư �̺�Ʈ �ְ� false
        //skillBuyButton.gameObject.SetActive(false);
    }

    private void OnClickBuySkillButton() //���� ��ư
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

    private void OnButtonClick(int num) //��ư ��������
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
            closeTimeText.text = $"���� ���۱��� {second}��";
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

