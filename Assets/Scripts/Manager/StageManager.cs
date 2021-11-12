using UnityEngine;

public class StageManager : MonoBehaviour //현제 스테이지의 정보를 가지고있다.
{
    public int nowStage;

    public Stage[] stages;

    private int curStage;

    private PlayerHealth playerHealth;

    public void Start()
    {
        curStage = 0;
        stages[curStage].Play();
    }


    public bool OnClearStage()
    {
        curStage++;

        if ((curStage) == stages.Length)
        {
            print("전체 스테이지 클리어");
            SaveData();
            return true;
        }
        else
        {
            stages[curStage - 1].potal.SetEvent(() =>
            {
                GameManager.Instance.FadeInOut(() =>
                {
                    stages[curStage].Play();
                    stages[curStage - 1].Stop();
                });
            });


            print(curStage + "스테이지 클리어");

            return false;
            //포탈 열여야해
        }
    }

    private void SaveData()
    {
        if (playerHealth == null)
        {
            playerHealth = GameManager.Instance.GetPlayer().GetComponent<PlayerHealth>();
        }

        float remianHpPersent = (playerHealth.CurrentHealth / playerHealth.MaxHealth) * 100;

        StageVO vo = new StageVO(nowStage, remianHpPersent, GameManager.Instance.PlayTime);
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
