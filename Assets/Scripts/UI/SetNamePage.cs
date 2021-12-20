using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SetNamePage : MonoBehaviour
{
    public Button registerBtn;
    public InputField nameInput;
    public Text noticeText;

    public void Awake()
    {
        registerBtn.onClick.AddListener(() =>
        {
            Regex reg = new Regex(@"^[��-�Ra-zA-Z]{2,4}$");

            //   @"^[0-9]{3}-[0-9]{3,4}-[0-9]{4}$";
            if (!reg.IsMatch(nameInput.text))
            {
                noticeText.text = "�̸��� �ѱ� �Ǵ� �������� 2~4���ڿ��� �մϴ�.";
                return;
            }

            UserVO vo = new UserVO(SystemInfo.deviceUniqueIdentifier, nameInput.text);
            string json = JsonUtility.ToJson(vo);

            NetworkManager.instance.SendPostRequest("checkoverlapName", json, result =>
            {
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
                if (vo.result)
                {
                    noticeText.text = "";
                }
                else
                {
                    noticeText.text = "�ߺ��� �̸��Դϴ�.";
                    return;
                }
            });

            NetworkManager.instance.SendPostRequest("register", json, result =>
            {
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
                if (vo.result)
                {
                    //ȸ������ �Ϸ�
                    noticeText.text = "ȸ������ �Ϸ�";
                    NetworkManager.instance.SetToken(vo.payload); //��ū ����
                    LoadingSceneManager.LoadScene("MainLobby");
                }
                else
                {
                    //ȸ������ ����
                    print(vo.payload);
                    print(vo.msg);
                    noticeText.text = "ȸ������ ����";
                }
            });
        });
    }
}
