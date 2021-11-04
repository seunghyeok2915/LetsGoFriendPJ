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
        stageManager = GetComponent<StageManager>();
    }

    protected virtual void OnDestroy()
    {
        _instance = default;
    }

    public UIGameClearPanel uiGameClearPanel;
    private List<GameObject> enemyListInStage = new List<GameObject>();
    private PlayerStats player;
    private StageManager stageManager;
    private bool isCaught = false;
    private bool isPlaying = true;

    public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
    public bool IsCaught { get => isCaught; set => isCaught = value; }

    private void Start()
    {
        PoolManager.CreatePool<Shuriken>("Shuriken1", this.gameObject, 5);
        PoolManager.CreatePool<TurretBullet>("TurretBullet", this.gameObject, 5);
        PoolManager.CreatePool<BulletHitGroundEffect>("BulletHitGroundEffect", this.gameObject, 5);
        PoolManager.CreatePool<FireEffect>("FireEffect", this.gameObject, 5);
    }

    private void Update()
    {
        if (enemyListInStage.Count <= 0 && IsPlaying)
        {
            IsPlaying = false;
            //TODO : 게임 클리어
            stageManager.OnClearStage();
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

    public PlayerStats GetPlayer() => player;
    public List<GameObject> GetEnemyListInStage() => enemyListInStage;

    private void FindPlayer()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainLobby");
    }
}
