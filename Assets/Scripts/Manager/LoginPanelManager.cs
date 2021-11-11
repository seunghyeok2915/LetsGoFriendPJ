using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginPanelManager : MonoBehaviour
{
    private void Start()
    {
        if (NetworkManager.instance.HasToken())
        {
            NetworkManager.instance.SendGetRequest("checkserver", "", result =>
            {
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
                if (vo.result)
                {
                    print(vo.payload);
                    LoadingSceneManager.LoadScene("MainLobby");
                }
                else
                {
                    print("½ÇÆÐ");
                }
            });
        }
    }
}
