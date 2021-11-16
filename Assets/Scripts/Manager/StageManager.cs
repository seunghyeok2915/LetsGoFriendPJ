using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class StageRange
{
    public int minRange;
    public int maxRange;

    public List<Stage> stages;

    public Stage PlayRandom()
    {
        int randNum = Random.Range(0, stages.Count);
        stages[randNum].Play();

        Stage stage = stages[randNum];

        stages.Remove(stages[randNum]);

        return stage;
    }
}

public class StageManager : MonoBehaviour //현제 스테이지의 정보를 가지고있다.
{
    public int nowChapter;

    public StageRange[] stageRanges;

    private Stage lastStage;

    private int curStage;
    private int maxStage = 0;

    private PlayerHealth playerHealth;

    public int CurStage { get => curStage; private set => curStage = value; }

    public void Start()
    {
        foreach (StageRange item in stageRanges)
        {
            maxStage += item.stages.Count;
        }
        CurStage = 0;
        PlayNext();

    }

    private void PlayNext()
    {
        lastStage = GetNowRange().PlayRandom();
    }

    private StageRange GetNowRange()
    {
        foreach (StageRange item in stageRanges)
        {
            if (curStage >= item.minRange && curStage <= item.maxRange)
            {
                return item;
            }
        }

        return null;
    }


    public bool OnClearStage()
    {
        CurStage++;
        SaveData();

        if ((CurStage) == maxStage)
        {
            print("전체 스테이지 클리어");
            return true;
        }
        else
        {
            lastStage.potal.SetEvent(() =>
            {
                GameManager.Instance.FadeInOut(() =>
                {
                    lastStage.Stop();
                    PlayNext();
                    print("꺼야함");
                });
            });


            print(CurStage + "스테이지 클리어");

            return false;
            //포탈 열여야해
        }
    }

    private void SaveData()
    {
        if (NetworkManager.instance == null)
        {
            return;
        }

        UserChapterVO vo = new UserChapterVO(nowChapter, curStage);
        string json = JsonUtility.ToJson(vo);

        print(json);
        NetworkManager.instance.SendPostRequest("updatetstage", json, result =>
        {
            ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
            if (vo.result)
            {
                print(vo.payload);
            }
            else
            {
                print("실패");
            }
        });

    }
}
