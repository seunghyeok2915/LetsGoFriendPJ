using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIMainLobby : MonoBehaviour
{
    public UISettingPanel uiSettingPanel;
    public UIRankPanel uiRankPanel;
    public ShopPage shopPage;

    public Button startBtn;
    public Button logoutBtn;
    public Button getDataButton;

    public Button shopBtn;

    public Button rankBtn;
    public Button settingBtn;

    public Button rightStageBtn;
    public Button leftStageBtn;

    public Text nameText;
    public Text zemText;

    public Image fadeImage;

    private void Start()
    {
        startBtn.onClick.AddListener(() => SoundManager.Instance.PlayFXSound("start"));
        logoutBtn.onClick.AddListener(NetworkManager.instance.Logout);
        rankBtn.onClick.AddListener(() => uiRankPanel.Open());
        settingBtn.onClick.AddListener(() => uiSettingPanel.gameObject.SetActive(true));
        shopBtn.onClick.AddListener(() => shopPage.gameObject.SetActive(true));
    }

    public void UpdateStageBtn(int maxStage,int nowStage)
    {
        if(maxStage == nowStage)
        {
            rightStageBtn.gameObject.SetActive(false);
        }
        else
        {
            rightStageBtn.gameObject.SetActive(true);
            leftStageBtn.gameObject.SetActive(true);
        }

        if (nowStage == 1)
        {
            leftStageBtn.gameObject.SetActive(false);
        }
        else
        {
            leftStageBtn.gameObject.SetActive(true);
        }
    }

    public void UpdateZem(string name,int zem)
    {
        nameText.text = name;
        zemText.text = zem.ToString();
    }


}
