using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginPage : MonoBehaviour
{
    public Button loginButton;
    public InputField loginInputField;
    public InputField passwordInputField;

    private void Start()
    {
        loginButton.onClick.AddListener(() =>
        {
            //여기서 id와 pass에 올바르지 않은문자나 공백이 있는 지 검사해야해
            LoginVO vo = new LoginVO(loginInputField.text, passwordInputField.text);
            string payload = JsonUtility.ToJson(vo);

            NetworkManager.instance.SendPostRequest("login", payload, res =>
            {
                Debug.Log(res);
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(res);
                if (vo.result)
                {
                    //PopUpManager.instance.OpenPopUp("alert", "로그인 성공", 2);
                    print("로그인 성공");
                    //NetworkManager.instance.SetToken(vo.payload); //토큰 저장
                }
                else
                {
                    print("로그인 실패");
                    //PopUpManager.instance.OpenPopUp("alert", vo.payload, 1);
                }
            });
        });
    }
}
