using DG.Tweening;
using UnityEngine;
public class LobbyManager : MonoBehaviour
{
    public UIMainLobby uiMainLobby;
    public PlayerLobby playerLobby;
    public UIStarPanel starPanel;

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
            OpenStar();
            uiMainLobby.UpdateStageBtn(playerStage, nowStage);
        });

        uiMainLobby.leftStageBtn.onClick.AddListener(() =>
        {
            if (playerLobby.animator.GetBool("isMoving"))
            {
                return;
            }
            nowStage -= 1;
            OpenStar();
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

    public void OpenStar()
    {
        StageVO vo = new StageVO(nowStage, 0, 0);
        string json = JsonUtility.ToJson(vo);
        print("스타실행");
        print(json);
        NetworkManager.instance.SendPostRequest("getuserstagedata", json, result =>
        {
            ResponseVO res = JsonUtility.FromJson<ResponseVO>(result);

            print(res.payload);
            print(res.result);

            if (res.result)
            {
                print(res.payload);
                print(int.Parse(res.payload));

                starPanel.Open(int.Parse(res.payload));
            }
            else
            {
                print("실패");
            }
        });
        NetworkManager.instance.SendPostRequest("getstagedata", json, result =>
        {
            ResponseVO res = JsonUtility.FromJson<ResponseVO>(result);

            print(res.payload +"1111");

            if (res.result)
            {
                StageVO vo = JsonUtility.FromJson<StageVO>(res.payload);
                starPanel.Open(vo);
            }
            else
            {
                print("실패");
            }
        });
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
        OpenStar();
    }

}
