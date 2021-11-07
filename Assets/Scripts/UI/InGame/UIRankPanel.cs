using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRankPanel : MonoBehaviour
{
    public Button openBtn;
    public Button closeBtn;

    public Transform contentView;
    public RankInfo rankItemPrefab; //��ũ ������ ������

    public void Start()
    {
        openBtn.onClick.AddListener(Open);
        closeBtn.onClick.AddListener(Close);

        Close();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        NetworkManager.instance.SendGetRequest("ranklist", "", ListGenerate);

    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void ListGenerate(string json)
    {
        //Debug.Log(json);
        //1. json�� ���� �ڷᱸ�� (�� VO���¸� ��������)
        //2. jsonutility�� �̿��ؼ� json�� vo�� �Ľ���
        ResponseVO res = JsonUtility.FromJson<ResponseVO>(json);

        Debug.Log(json);
        if (res.result)
        {
            RecordListVO vo = JsonUtility.FromJson<RecordListVO>(res.payload);
            for (int i = contentView.childCount - 1; i >= 0; i--)
            {
                Destroy(contentView.GetChild(i).gameObject);
            }

            if (vo == null)
            {
                print(res.payload);
            }
            for (int i = 0; i < vo.list.Count; i++)
            {
                RankInfo ris = Instantiate(rankItemPrefab, contentView);
                ris.SetData(i + 1, vo.list[i]);
                ris.SetAnimation(i * 0.1f + 0.1f);
            }
        }
        else
        {
            Debug.Log(res.payload);
        }



        //3. �Ľ��� vo���� list�� for���� ���鼭 Instantiate �ؼ� contentView�� �ڽ����� �־�� ��
        //  �� ���۾��� �ϱ����� contentView�� ��� �ڽ��� �����ؾ� �Ѵ�. 
        //  Destroy �� Instantiate�� �Ἥ ��ũ�� ǥ�õǵ��� �Ѵ�.


    }
}
