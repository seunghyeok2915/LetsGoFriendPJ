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
                NetworkManager.instance.SetToken(vo.payload); //토큰 저장
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
                print("실패");
            }
        });

        if (NetworkManager.instance.HasToken())
        {
            LoadingSceneManager.LoadScene("MainLobby");
        }
    }
}
