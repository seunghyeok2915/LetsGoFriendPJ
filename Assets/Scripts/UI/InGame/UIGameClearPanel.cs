using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameClearPanel : MonoBehaviour
{
    public Text clearText;
    public Text clearTime;
    public Button confirmButton;
    public Button adButton;
    public Text zemText;

    public CanvasGroup canvasGroup;

    private void Start()
    {
        Close();

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    private void OnClickAdBtn()
    {
        NetworkManager.instance.UpdateZem(GameManager.Instance.EarnZem * 2, () => LoadingSceneManager.LoadScene("MainLobby"));

    }

    private void OnClickConfirmButton()
    {
        NetworkManager.instance.UpdateZem(GameManager.Instance.EarnZem, () => LoadingSceneManager.LoadScene("MainLobby"));
    }

    private void RegisterButtons()
    {
        confirmButton.onClick.RemoveAllListeners();
        adButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(OnClickConfirmButton);
        adButton.onClick.AddListener(OnClickAdBtn);
    }

    public void PopUp(int stage)
    {
        RegisterButtons();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        zemText.text = GameManager.Instance.EarnZem.ToString();
        clearText.text = $"{stage} 스테이지 클리어";
        clearTime.text = $"클리어 시간 : {GameManager.Instance.PlayTime.ToString("00:00")}초";
        canvasGroup.DOFade(1, 1f);
    }

    private void Close()
    {
        canvasGroup.alpha = 0f;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
