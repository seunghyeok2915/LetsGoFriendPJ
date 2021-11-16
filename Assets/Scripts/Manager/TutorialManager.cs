using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Transform TutorialChild;
    public UIStatsPanel statsPanel;
    public SlotSkill eskill;
    private int curTut = 0;

    private bool isStarted;

    public void Start()
    {
        Invoke(nameof(StartTutorial), 3f);
    }

    public void StartTutorial()
    {
        Time.timeScale = 0f;
        isStarted = true;
        SetActive(0);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isStarted)
        {
            PrintNext();
        }
    }

    private void PrintNext()
    {
        SetActive(curTut + 1);
        curTut++;

        if (curTut == TutorialChild.childCount)
        {
            Time.timeScale = 1;
            GameManager.Instance.CheckEnd();
        }
    }

    private void SetActive(int value)
    {
        for (int i = 0; i < TutorialChild.childCount; i++)
        {
            TutorialChild.GetChild(i).gameObject.SetActive(i == value);
        }

        if(value == 6)
        {
            statsPanel.AddSkill(eskill);
            GameManager.Instance.GetPlayer().AddSkill(ESkill.Boomerang);
        }
    }
}
