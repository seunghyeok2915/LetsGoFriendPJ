using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UIRegisterPage : PopUp
{
    public Button registerBtn;
    public Button closeBtn;
    public Button overlapCheckBtn;

    public InputField nameInput;
    public InputField idInput;

    public InputField passInput;
    public InputField passOverlapCheckInput;

    protected override void Awake()
    {
        base.Awake();

        overlapCheckBtn.onClick.AddListener(() =>
        {
            RegisterVO vo = new RegisterVO(idInput.text, passInput.text, nameInput.text);
            string json = JsonUtility.ToJson(vo);
            NetworkManager.instance.SendPostRequest("checkoverlapID", json, result =>
            {
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
                if (vo.result)
                {
                    PopUpManager.instance.OpenPopUp("alert", vo.payload);
                    registerBtn.interactable = true;
                }
                else
                {
                    PopUpManager.instance.OpenPopUp("alert", vo.payload);
                }
            });
        });

        registerBtn.onClick.AddListener(() =>
        {
            Regex reg = new Regex(@"^[��-�Ra-zA-Z]{2,6}$");

            //   @"^[0-9]{3}-[0-9]{3,4}-[0-9]{4}$";
            if (!reg.IsMatch(nameInput.text))
            {
                Debug.Log("�̸��� �ݵ�� �ѱ� �Ǵ� �������� 2~6���ڿ��� �մϴ�.");
                PopUpManager.instance.OpenPopUp(
                    "alert", "�̸��� �ݵ�� �ѱ� �Ǵ� �������� 2~6���ڿ��� �մϴ�.");
                return;
            }

            //��ĭ�� �ִ°�?

            //passInput�� passConfirm �� �ٸ��� Debug.Error ��쵵�� �ϰ� => popup���� ��ü
            if (passInput.text != passOverlapCheckInput.text || string.IsNullOrWhiteSpace(passInput.text))
            {
                PopUpManager.instance.OpenPopUp("alert", "��й�ȣ�� �����̰ų� �ٸ��ϴ�.");
                return;
            }

            RegisterVO vo = new RegisterVO(idInput.text, passInput.text, nameInput.text);
            string json = JsonUtility.ToJson(vo);
            NetworkManager.instance.SendPostRequest("register", json, result =>
            {
                //ResponseVO ���·� result�� �Ľ��ؼ�
                // �� ����� true��� ���� Ŭ���� ȸ������â�� ���� ������
                // false��� �� Ŭ���� �󷵸� ������
                

                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
                if (vo.result)
                {
                    PopUpManager.instance.OpenPopUp("alert", vo.payload, 2);
                }
                else
                {
                    PopUpManager.instance.OpenPopUp("alert", vo.payload);
                }
            });
        });

        closeBtn.onClick.AddListener(() => PopUpManager.instance.ClosePopUp());
        registerBtn.interactable = false;
    }
}
