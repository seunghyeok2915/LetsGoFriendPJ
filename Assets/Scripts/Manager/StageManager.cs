using UnityEngine;

public class StageManager : MonoBehaviour //현제 스테이지의 정보를 가지고있다.
{
    public int nowChapter;

    public Stage[] stages;

    private int curStage;

    private PlayerHealth playerHealth;

    public int CurStage { get => curStage; set => curStage = value; }

    public void Start()
    {
        CurStage = 0;
        stages[CurStage].Play();
    }


    public bool OnClearStage()
    {
        CurStage++;
        SaveData();

        if ((CurStage) == stages.Length)
        {
            print("전체 스테이지 클리어");
            return true;
        }
        else
        {
            stages[CurStage - 1].potal.SetEvent(() =>
            {
                GameManager.Instance.FadeInOut(() =>
                {
                    stages[CurStage].Play();
                    stages[CurStage - 1].Stop();
                });
            });


            print(CurStage + "스테이지 클리어");

            return false;
            //포탈 열여야해
        }
    }

    private void SaveData()
    {
        UserChapterVO vo = new UserChapterVO(nowChapter,curStage);
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
