using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

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

        if(canvasGroup == null)
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void UpdateZem(int zem)
    {
        UserDataVO vo = new UserDataVO("",zem, 0, 0);

        string json = JsonUtility.ToJson(vo);

        NetworkManager.instance.SendPostRequest("updatezem", json, result =>
        {
            ResponseVO res = JsonUtility.FromJson<ResponseVO>(result);

            if (res.result)
            {
                print(res.payload);
            }
            else
            {
                print(res.payload);
            }

            SceneManager.LoadScene("MainLobby");
        });
    }

    private void OnClickAdBtn()
    {
        UpdateZem(GameManager.Instance.EarnZem * 2);
    }

    private void OnClickConfirmButton()
    {
        UpdateZem(GameManager.Instance.EarnZem);
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
