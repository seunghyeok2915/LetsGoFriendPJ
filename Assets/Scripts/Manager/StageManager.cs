using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour //���� ���������� ������ �������ִ�.
{
    public int nowStage;

    public void OnClearStage()
    {
        StageVO vo = new StageVO(nowStage + 1);
        string json = JsonUtility.ToJson(vo);
        NetworkManager.instance.SendPostRequest("insertstage", json, result =>
        {
            //ResponseVO ���·� result�� �Ľ��ؼ�
            // �� ����� true��� ���� Ŭ���� ȸ������â�� ���� ������
            // false��� �� Ŭ���� �󷵸� ������
             
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
