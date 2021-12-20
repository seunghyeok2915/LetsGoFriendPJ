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
            Regex reg = new Regex(@"^[°¡-ÆRa-zA-Z]{2,4}$");

            //   @"^[0-9]{3}-[0-9]{3,4}-[0-9]{4}$";
            if (!reg.IsMatch(nameInput.text))
            {
                noticeText.text = "ÀÌ¸§Àº ÇÑ±Û ¶Ç´Â ¿µ¹®À¸·Î 2~4±ÛÀÚ¿©¾ß ÇÕ´Ï´Ù.";
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
                    noticeText.text = "Áßº¹µÈ ÀÌ¸§ÀÔ´Ï´Ù.";
                    return;
                }
            });

            NetworkManager.instance.SendPostRequest("register", json, result =>
            {
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
                if (vo.result)
                {
                    //È¸¿ø°¡ÀÔ ¿Ï·á
                    noticeText.text = "È¸¿ø°¡ÀÔ ¿Ï·á";
                    NetworkManager.instance.SetToken(vo.payload); //ÅäÅ« ÀúÀå
                    LoadingSceneManager.LoadScene("MainLobby");
                }
                else
                {
                    //È¸¿ø°¡ÀÔ ½ÇÆÐ
                    print(vo.payload);
                    print(vo.msg);
                    noticeText.text = "È¸¿ø°¡ÀÔ ½ÇÆÐ";
                }
            });
        });
    }
}
