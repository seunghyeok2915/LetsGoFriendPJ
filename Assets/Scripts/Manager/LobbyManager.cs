using DG.Tweening;
using UnityEngine;
public class LobbyManager : MonoBehaviour
{
    public UIMainLobby uiMainLobby;
    public PlayerLobby playerLobby;
    public string playerName;
    public int playerStage;
    public int playerZem;
    public int playerScore;

    public int nowStage;
    private void Start()
    {
        GetData();

        uiMainLobby.rightStageBtn.onClick.AddListener(() =>
        {
            if (playerLobby.animator.GetBool("isMoving"))
            {
                return;
            }
            nowStage += 1;
            uiMainLobby.UpdateStageBtn(playerStage, nowStage);
        });

        uiMainLobby.leftStageBtn.onClick.AddListener(() =>
        {
            if (playerLobby.animator.GetBool("isMoving"))
            {
                return;
            }
            nowStage -= 1;
            uiMainLobby.UpdateStageBtn(playerStage, nowStage);
        });

        uiMainLobby.startBtn.onClick.AddListener(() =>
        {
            if (playerLobby.animator.GetBool("isMoving"))
            {
                return;
            }

            uiMainLobby.startBtn.gameObject.SetActive(false);
            uiMainLobby.fadeImage.DOFade(1, 4f);
        });

        uiMainLobby.rightStageBtn.onClick.AddListener(playerLobby.MoveRight);
        uiMainLobby.leftStageBtn.onClick.AddListener(playerLobby.MoveLeft);
        uiMainLobby.startBtn.onClick.AddListener(playerLobby.MoveBack);
    }



    public void GetData()
    {
        NetworkManager.instance.SendGetRequest("getuserdata", "", GetData);
    }

    private void GetData(string json)
    {
        ResponseVO res = JsonUtility.FromJson<ResponseVO>(json);

        Debug.Log(json);
        if (res.result)
        {
            UserDataVO vo = JsonUtility.FromJson<UserDataVO>(res.payload);

            playerName = vo.name;
            playerZem = vo.zem;
            playerStage = vo.stage;
            playerScore = vo.score;

            nowStage = playerStage;
        }
        else
        {
            Debug.Log(res.payload);
        }

        uiMainLobby.UpdateZem(playerName, playerZem);
        uiMainLobby.UpdateStageBtn(playerStage, nowStage);

        playerLobby.SetPlayerPos();
    }
}
