using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public string baseUrl = "0";

    private string token = "";

    public void SetToken(string token)
    {
        this.token = token;
        PlayerPrefs.SetString("token", token); //토큰을 저장
        SceneManager.LoadScene("MainLobby");
        //UIManager.instance.ShowBox1();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 NetworkManager가 돌아가고 있습니다.");
            Destroy(this);
        }
        instance = this;
        token = PlayerPrefs.GetString("token", ""); //없으면 null나옴
        DontDestroyOnLoad(this);
    }

    public bool HasToken()
    {
        return !token.Equals("");
    }

    public void Logout()
    {
        //토큰을 null로 변경하고 PlayerPrefabs에서 token을 지워주고
        token = "";
        PlayerPrefs.DeleteKey("token");
        SceneManager.LoadScene("LoginScene");
        Destroy(gameObject);
        //UIManager.instance.ShowBox1();
        // showBox1을 하면 된다.
    }

    public void UpdateZem(int zem,UnityAction unityAction = null)
    {
        UserDataVO vo = new UserDataVO("", zem, 0, 0);

        string json = JsonUtility.ToJson(vo);

        SendPostRequest("updatezem", json, result =>
        {
            ResponseVO res = JsonUtility.FromJson<ResponseVO>(result);

            if (res.result)
            {
                print(res.payload);
            }
            else
            {
                print(res.payload);
            }
            unityAction?.Invoke();
        });
    }

    public void AddZem(int zem, UnityAction unityAction = null)
    {
        UserDataVO vo = new UserDataVO("", zem, 0, 0);

        string json = JsonUtility.ToJson(vo);

        SendPostRequest("addzem", json, result =>
        {
            ResponseVO res = JsonUtility.FromJson<ResponseVO>(result);

            if (res.result)
            {
                print(res.payload);
            }
            else
            {
                print(res.payload);
            }
            unityAction?.Invoke();
        });
    }

    public void SendGetRequest(string url, string queryString, Action<string> callBack)
    { 
            StartCoroutine(SendGet($"{baseUrl}/{url}{queryString}", callBack));
    }

    public void SendPostRequest(string url, string payload, Action<string> callBack)
    {
        StartCoroutine(SendPost($"{baseUrl}/{url}", payload, callBack));
    }

    IEnumerator SendGet(string url, Action<string> callBack)
    {
        UnityWebRequest req = UnityWebRequest.Get(url);

        req.SetRequestHeader("authorization", "Bearer " + token);

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string data = req.downloadHandler.text;
            callBack(data);
        }
        else
        {
            callBack("{\"result\":false, \"msg\": \"error in communication\"}");
        }
    }

    IEnumerator SendPost(string url, string payload, Action<string> callBack)
    {
        UnityWebRequest req = UnityWebRequest.Post(url, payload);
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("authorization", "Bearer " + token);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(payload);
        req.uploadHandler = new UploadHandlerRaw(jsonToSend);

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string data = req.downloadHandler.text;
            callBack(data);
        }
        else
        {
            callBack("{\"result\":false, \"msg\": \"error in communicaion\"}");
        }
    }
}
