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
            //���⼭ id�� pass�� �ùٸ��� �������ڳ� ������ �ִ� �� �˻��ؾ���
            LoginVO vo = new LoginVO(loginInputField.text, passwordInputField.text);
            string payload = JsonUtility.ToJson(vo);

            NetworkManager.instance.SendPostRequest("login", payload, res =>
            {
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(res);
                if (vo.result)
                {
                    PopUpManager.instance.OpenPopUp("alert", "�α��� ����", 2);
                    NetworkManager.instance.SetToken(vo.payload); //��ū ����
                    print("�α��� ����");
                }
                else
                {
                    if(!string.IsNullOrEmpty(vo.msg))
                    {
                        PopUpManager.instance.OpenPopUp("alert", vo.msg, 1);
                        return;
                    }
                    PopUpManager.instance.OpenPopUp("alert", vo.payload, 1);

                    print("�α��� ����");
                }
            });
        });
        registerButton.onClick.AddListener(() => PopUpManager.instance.OpenPopUp("register"));
        findIDButton.onClick.AddListener(() => PopUpManager.instance.OpenPopUp("alert","�����ڿ��� �����ϼ���."));
        resetPasswordButton.onClick.AddListener(() => PopUpManager.instance.OpenPopUp("alert", "�����ڿ��� �����ϼ���."));
    }
}
