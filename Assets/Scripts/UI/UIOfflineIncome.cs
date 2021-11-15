using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIOfflineIncome : MonoBehaviour
{
    public LobbyManager lobby;
    public GameObject popUpPanel;

    public Text adClaimTxt;
    public Text claimTxt;

    public Button adClaimBtn;
    public Button claimBtn;

    private int gold;

    private Sequence seq1;

    public void Popup(int time)
    {
        gameObject.SetActive(true);

        seq1.Kill();
        seq1 = DOTween.Sequence();

        seq1.Append(popUpPanel.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0));

        seq1.Append(popUpPanel.transform.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.OutElastic));

        gold = (int)(time * 0.00014);

        claimTxt.text = string.Format($"{gold}");
        adClaimTxt.text = string.Format($"{gold * 2}");

        adClaimBtn.onClick.RemoveAllListeners();
        claimBtn.onClick.RemoveAllListeners();

        adClaimBtn.onClick.AddListener(() => OnClickClaimBtn(gold *2));
        claimBtn.onClick.AddListener(() => OnClickClaimBtn(gold));
    }

    public void OnClickClaimBtn(int gold)
    {
        //SoundManager.instance.PlaySound(6);
        //GameManager.instance.Vibrate();

        NetworkManager.instance.AddZem(gold, lobby.GetData);
        

        popUpPanel.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f).OnComplete(() => gameObject.SetActive(false));

    }
}
