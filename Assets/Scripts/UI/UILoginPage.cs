using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginPage : MonoBehaviour
{
    public Button loginButton;
    public InputField loginInputField;
    public InputField passwordInputField;

    public Button findIDButton;
    public Button resetPasswordButton;
    public Button registerButton;

    private void Start()
    {
        loginButton.onClick.AddListener(() =>
        {
            //여기서 id와 pass에 올바르지 않은문자나 공백이 있는 지 검사해야해
            LoginVO vo = new LoginVO(loginInputField.text, passwordInputField.text);
            string payload = JsonUtility.ToJson(vo);

            NetworkManager.instance.SendPostRequest("login", payload, res =>
            {
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(res);
                if (vo.result)
                {
                    PopUpManager.instance.OpenPopUp("alert", "로그인 성공", 2);
                    NetworkManager.instance.SetToken(vo.payload); //토큰 저장
                    print("로그인 성공");
                }
                else
                {
                    if(!string.IsNullOrEmpty(vo.msg))
                    {
                        PopUpManager.instance.OpenPopUp("alert", vo.msg, 1);
                        return;
                    }
                    PopUpManager.instance.OpenPopUp("alert", vo.payload, 1);

                    print("로그인 실패");
                }
            });
        });
        registerButton.onClick.AddListener(() => PopUpManager.instance.OpenPopUp("register"));
        findIDButton.onClick.AddListener(() => PopUpManager.instance.OpenPopUp("alert","관리자에게 문의하세요."));
        resetPasswordButton.onClick.AddListener(() => PopUpManager.instance.OpenPopUp("alert", "관리자에게 문의하세요."));
    }
}
