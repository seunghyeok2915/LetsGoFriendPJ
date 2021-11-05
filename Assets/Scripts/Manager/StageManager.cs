using UnityEngine;

public class StageManager : MonoBehaviour //���� ���������� ������ �������ִ�.
{
    public int nowStage;
    private PlayerHealth playerHealth;

    public void OnClearStage()
    {
        print("�������� Ŭ����");

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
