using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  UnityEngine.SceneManagement;

public class UIMainLobby : MonoBehaviour
{
    public Button startBtn;

    private void Start()
    {
        startBtn.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
    }
}
