using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UISettingPanel : MonoBehaviour
{
    public Image settingPanel;
    public GameObject popupPanel;

    public Button soundBtn;
    public Button hapticBtn;

    public Text soundTxt;
    public Text hapticTxt;

    public Text appVersionTxt;

    public Button tutorialBtn;

    public Button closeBtn;

    private Sequence seq1;

    private void Start()
    {
        soundBtn.onClick.AddListener(OnClickSoundBtn);
        hapticBtn.onClick.AddListener(OnClickHapticBtn);
        tutorialBtn.onClick.AddListener(OnClickTutotialBtn);

        closeBtn.onClick.AddListener(CallCloseFunc);
    }

    private void OnEnable()
    {
        settingPanel.DOFade(0, 0f);
        settingPanel.DOFade(1, 1f);

        seq1.Kill();
        seq1 = DOTween.Sequence();

        seq1.Append(popupPanel.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0));

        seq1.Append(popupPanel.transform.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.OutElastic));

        //soundTxt.text = GameManager.instance.sound ? "ON" : "OFF";
        //hapticTxt.text = GameManager.instance.haptic ? "ON" : "OFF";

        appVersionTxt.text = Application.version;
    }

    private void OnClickTutotialBtn()
    {
        LoadingSceneManager.LoadScene("TutorialGame");
    }
    private void OnClickSoundBtn()
    {
        //SoundManager.instance.PlaySound(6);
        //GameManager.instance.Vibrate();

        //GameManager.instance.sound = !GameManager.instance.sound;
        //soundTxt.text = GameManager.instance.sound ? "ON" : "OFF";

        //SoundManager.instance.OnChangeVolume();

        //DataManager.SaveData();
        int sound = 1;
        if (PlayerPrefs.HasKey("sound"))
        {
            sound = PlayerPrefs.GetInt("sound");
        }

        if(sound == 0)
        {
            sound = 1;
            soundTxt.text = "On";
        }
        else
        {
            sound = 0;
            soundTxt.text = "Off";
        }
        PlayerPrefs.SetInt("sound", sound);
        SoundManager.Instance.AdjustMasterVolume(sound);

    }

    private void OnClickHapticBtn()
    {
        //SoundManager.instance.PlaySound(6);
        //GameManager.instance.Vibrate();

        //GameManager.instance.haptic = !GameManager.instance.haptic;
        //hapticTxt.text = GameManager.instance.haptic ? "ON" : "OFF";

        //DataManager.SaveData();

        int haptic = 1;
        if (PlayerPrefs.HasKey("haptic"))
        {
            haptic = PlayerPrefs.GetInt("haptic");
        }

        if (haptic == 0)
        {
            haptic = 1;
            hapticTxt.text = "On";
        }
        else
        {
            haptic = 0;
            hapticTxt.text = "Off";
        }
        PlayerPrefs.SetInt("haptic", haptic);
    }

    private void CallCloseFunc()
    {
        //SoundManager.instance.PlaySound(6);
        //GameManager.instance.Vibrate();
        gameObject.SetActive(false);
    }

}
