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



        //3. 파싱한 vo에서 list를 for문을 돌면서 Instantiate 해서 contentView의 자식으로 넣어야 해
        //  단 이작업을 하기전에 contentView의 모든 자식을 삭제해야 한다. 
        //  Destroy 와 Instantiate를 써서 랭크가 표시되도록 한다.


    }
}
