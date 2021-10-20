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
            Regex reg = new Regex(@"^[°¡-ÆRa-zA-Z]{2,6}$");

            //   @"^[0-9]{3}-[0-9]{3,4}-[0-9]{4}$";
            if (!reg.IsMatch(nameInput.text))
            {
                Debug.Log("ÀÌ¸§Àº ¹Ýµå½Ã ÇÑ±Û ¶Ç´Â ¿µ¹®À¸·Î 2~6±ÛÀÚ¿©¾ß ÇÕ´Ï´Ù.");
                PopUpManager.instance.OpenPopUp(
                    "alert", "ÀÌ¸§Àº ¹Ýµå½Ã ÇÑ±Û ¶Ç´Â ¿µ¹®À¸·Î 2~6±ÛÀÚ¿©¾ß ÇÕ´Ï´Ù.");
                return;
            }

            //ºóÄ­ÀÌ ÀÖ´Â°¡?

            //passInput°ú passConfirm ÀÌ ´Ù¸£¸é Debug.Error ¶ç¿ìµµ·Ï ÇÏ°í => popupÀ¸·Î ±³Ã¼
            if (passInput.text != passOverlapCheckInput.text || string.IsNullOrWhiteSpace(passInput.text))
            {
                PopUpManager.instance.OpenPopUp("alert", "ºñ¹Ð¹øÈ£°¡ °ø¹éÀÌ°Å³ª ´Ù¸¨´Ï´Ù.");
                return;
            }

            RegisterVO vo = new RegisterVO(idInput.text, passInput.text, nameInput.text);
            string json = JsonUtility.ToJson(vo);
            NetworkManager.instance.SendPostRequest("register", json, result =>
            {
                //ResponseVO ÇüÅÂ·Î result¸¦ ÆÄ½ÌÇØ¼­
                // ±× °á°ú°¡ true¶ó¸é ¾ó·µÀ» Å¬¸¯½Ã È¸¿ø°¡ÀÔÃ¢µµ °°ÀÌ ´ÝÈ÷°í
                // false¶ó¸é ¾ó·µ Å¬¸¯½Ã ¾ó·µ¸¸ ´ÝÈ÷°Ô
                

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
