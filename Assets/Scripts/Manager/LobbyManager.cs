using DG.Tweening;
using UnityEngine;
public class LobbyManager : MonoBehaviour
{
    public UIMainLobby uiMainLobby;
    public PlayerLobby playerLobby;
    public UIChapterInfoPanel uiChapterInfo;
    public UIOfflineIncome income;

    public string playerName;
    public int playerChapter;
    public int playerZem;
    public int playerTotal_score;

    public int nowChapter;
    private void Start()
    {
        GetData();

        if (PlayerPrefs.HasKey("outUnixTime"))
        {
            int offEarnTime = Utils.GetUnixTime() - PlayerPrefs.GetInt("outUnixTime");
            if (offEarnTime > 7200)
            {
                income.Popup(offEarnTime);
                PlayerPrefs.SetInt("outUnixTime",Utils.GetUnixTime());
            }
        }
        else
        {
            PlayerPrefs.SetInt("outUnixTime", Utils.GetUnixTime());
        }

        uiMainLobby.rightStageBtn.onClick.AddListener(() =>
        {
            if (playerLobby.animator.GetBool("isMoving"))
            {
                return;
            }
            nowChapter += 1;
            OpenStar();
            uiMainLobby.UpdateStageBtn(playerChapter, nowChapter);
        });

        uiMainLobby.leftStageBtn.onClick.AddListener(() =>
        {
            if (playerLobby.animator.GetBool("isMoving"))
            {
                return;
            }
            nowChapter -= 1;
            OpenStar();
            uiMainLobby.UpdateStageBtn(playerChapter, nowChapter);
        });

        uiMainLobby.startBtn.onClick.AddListener(() =>
        {
            if (playerLobby.animator.GetBool("isMoving"))
            {
                return;
            }

            uiMainLobby.startBtn.gameObject.SetActive(false);
            uiMainLobby.fadeImage.DOFade(1, 2f);
        });

        uiMainLobby.rightStageBtn.onClick.AddListener(playerLobby.MoveRight);
        uiMainLobby.leftStageBtn.onClick.AddListener(playerLobby.MoveLeft);
        uiMainLobby.startBtn.onClick.AddListener(playerLobby.MoveBack);
    }

    public void OpenStar()
    {
        ChapterVO vo = new ChapterVO(nowChapter, "", 0);
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

                uiChapterInfo.Open(int.Parse(res.payload));
            }
            else
            {
                print("실패");
            }
        });

        NetworkManager.instance.SendPostRequest("getstagedata", json, result =>
        {
            ResponseVO res = JsonUtility.FromJson<ResponseVO>(result);
            print("getstagedata : " + result);
            print(res.payload);

            if (res.result)
            {
                ChapterVO vo = JsonUtility.FromJson<ChapterVO>(res.payload);
                vo.id = nowChapter;
                uiChapterInfo.Open(vo);
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

        if (res.result)
        {
            UserDataVO vo = JsonUtility.FromJson<UserDataVO>(res.payload);

            playerName = vo.name;
            playerZem = vo.zem;
            playerChapter = vo.chapter;
            playerTotal_score = vo.total_stage;

            nowChapter = playerChapter;
        }
        else
        {
            Debug.Log(res.payload);
        }

        uiMainLobby.UpdateZem(playerName, playerZem);
        uiMainLobby.UpdateStageBtn(playerChapter, nowChapter);

        playerLobby.SetPlayerPos();
        OpenStar();
    }

}
