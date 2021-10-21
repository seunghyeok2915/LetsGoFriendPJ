using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainLobby : MonoBehaviour
{
    public Button startBtn;
    public Button logoutBtn;
    public Button getDataButton;

    public Text nameText;
    public Text zemText;

    private void Start()
    {
        startBtn.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
        logoutBtn.onClick.AddListener(NetworkManager.instance.Logout);
        NetworkManager.instance.SendGetRequest("getuserdata", "", GetData);

        getDataButton.onClick.AddListener(() =>
        {
            NetworkManager.instance.SendGetRequest("getuserdata", "", GetData);
        });
    }

    private void GetData(string json)
    {
        ResponseVO res = JsonUtility.FromJson<ResponseVO>(json);

        Debug.Log(json);
        if (res.result)
        {
            UserDataVO vo = JsonUtility.FromJson<UserDataVO>(res.payload);

            nameText.text = vo.name;
            zemText.text = vo.zem.ToString();
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
