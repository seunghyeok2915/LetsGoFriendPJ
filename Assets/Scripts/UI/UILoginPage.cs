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
            //���⼭ id�� pass�� �ùٸ��� �������ڳ� ������ �ִ� �� �˻��ؾ���
            LoginVO vo = new LoginVO(loginInputField.text, passwordInputField.text);
            string payload = JsonUtility.ToJson(vo);

            NetworkManager.instance.SendPostRequest("login", payload, res =>
            {
                Debug.Log(res);
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(res);
                if (vo.result)
                {
                    //PopUpManager.instance.OpenPopUp("alert", "�α��� ����", 2);
                    print("�α��� ����");
                    //NetworkManager.instance.SetToken(vo.payload); //��ū ����
                }
                else
                {
                    print("�α��� ����");
                    //PopUpManager.instance.OpenPopUp("alert", vo.payload, 1);
                }
            });
        });
    }
}
