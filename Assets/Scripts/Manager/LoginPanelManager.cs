using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginPanelManager : MonoBehaviour
{
    private void Start()
    {
        UserVO vo = new UserVO(SystemInfo.deviceUniqueIdentifier, "");
        string json = JsonUtility.ToJson(vo);
        NetworkManager.instance.SendPostRequest("checkid", json, result =>
        {
            ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
            if (vo.result)
            {
                NetworkManager.instance.SetToken(vo.payload); //��ū ����
                LoadingSceneManager.LoadScene("MainLobby");
            }
            else
            {

            }
        });

        NetworkManager.instance.SendGetRequest("checkserver", "", result =>
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

        if (NetworkManager.instance.HasToken())
        {
            LoadingSceneManager.LoadScene("MainLobby");
        }
    }
}
