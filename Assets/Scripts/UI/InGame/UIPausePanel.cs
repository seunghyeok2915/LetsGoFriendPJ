using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPausePanel : MonoBehaviour
{
    public Button exitBtn;
    public Button resumeBtn;

    public void Start()
    {
        exitBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.UpdateZem(GameManager.Instance.EarnZem, () => LoadingSceneManager.LoadScene("MainLobby"));
        });
        resumeBtn.onClick.AddListener(Close);
    }

    public void Open()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
