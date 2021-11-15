using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Transform TutorialChild;
    private int curTut = 0;

    private void Update()
    {
        Time.timeScale = 0f;
        if (Input.GetMouseButtonDown(0))
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
            gameObject.SetActive(false);
        }
    }

    private void SetActive(int value)
    {
        for (int i = 0; i < TutorialChild.childCount; i++)
        {
            TutorialChild.GetChild(i).gameObject.SetActive(i == value);
        }
    }
}
