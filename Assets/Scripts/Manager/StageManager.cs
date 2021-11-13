using UnityEngine;

public class StageManager : MonoBehaviour //���� ���������� ������ �������ִ�.
{
    public int nowStage;

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

        if ((CurStage) == stages.Length)
        {
            print("��ü �������� Ŭ����");
            SaveData();
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
                print("����");
            }
        });
    }
}
