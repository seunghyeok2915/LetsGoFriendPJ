    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as GameManager;
        }
        FindPlayer();
    }

    protected virtual void OnDestroy()
    {
        _instance = default;
    }

    public UIGameClearPanel uiGameClearPanel;
    private List<GameObject> enemyListInStage = new List<GameObject>();
    private GameObject player;
    private bool isCaught = false;
    private bool isPlaying = true;   
    public bool IsCaught
    {
        get { return isCaught; }
        set { isCaught = value; }
    }

    private void Start()
    {
        PoolManager.CreatePool<Shuriken>("Shuriken1", this.gameObject, 5);
        PoolManager.CreatePool<TurretBullet>("TurretBullet", this.gameObject, 5);
        PoolManager.CreatePool<BulletHitGroundEffect>("BulletHitGroundEffect", this.gameObject, 5);
    }

    private void Update()
    {
        if(enemyListInStage.Count <= 0 && isPlaying)
        {
            isPlaying = false;
            print("게임끝남");
            //TODO : 게임 클리어
            uiGameClearPanel.PopUp();

        }
    }



    public void AddEnemyInList(GameObject enemy)
    {
        enemyListInStage.Add(enemy);
    }

    public void RemoveEnemyInList(GameObject enemy)
    {
        enemyListInStage.Remove(enemy);
    }

    public GameObject GetPlayer() => player;
    public List<GameObject> GetEnemyListInStage() => enemyListInStage;

    private void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainLobby");
    }
}
