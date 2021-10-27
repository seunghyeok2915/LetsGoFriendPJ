using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIGameClearPanel : MonoBehaviour
{
    public Text gameText;
    public Button confirmButton;

    public CanvasGroup canvasGroup;

    private void Start()
    {
        Close();

        if(gameObject == null)
        canvasGroup = GetComponent<CanvasGroup>();

        confirmButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainLobby");
        });
    }

    public void PopUp()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        canvasGroup.DOFade(1, 1f);
    }

    private void Close()
    {
        canvasGroup.alpha = 0f;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
