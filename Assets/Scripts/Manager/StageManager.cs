using UnityEngine;

public class StageManager : MonoBehaviour //���� ���������� ������ �������ִ�.
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
            print("��ü �������� Ŭ����");
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


            print(CurStage + "�������� Ŭ����");

            return false;
            //��Ż ��������
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
                print("����");
            }
        });
    }
}
