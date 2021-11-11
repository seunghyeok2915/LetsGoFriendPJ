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
    public Canvas popupCanvas;

    private List<GameObject> enemyListInStage = new List<GameObject>();
    private PlayerStats player;
    private StageManager stageManager;
    private bool isCaught = false;
    private bool isPlaying = true;
    private bool isPotal = false;
    private float playTime = 0f;
    private int earnZem = 0;


    public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
    public bool IsCaught { get => isCaught; set => isCaught = value; }
    public float PlayTime { get => playTime; set => playTime = value; }
    public int EarnZem { get => earnZem; set => earnZem = value; }
    public bool IsPotal { get => isPotal; set => isPotal = value; }

    private void Start()
    {
        PoolManager.CreatePool<Shuriken>("Shuriken1", this.gameObject, 5);
        PoolManager.CreatePool<TurretBullet>("TurretBullet", this.gameObject, 5);
        PoolManager.CreatePool<BulletHitGroundEffect>("BulletHitGroundEffect", this.gameObject, 5);
        PoolManager.CreatePool<Effect>("CFX4 Fire", this.gameObject, 5);
        PoolManager.CreatePool<ExpCube>("ExpCube", this.gameObject, 20);
        PoolManager.CreatePool<PopupDamage>("PopupDamage", popupCanvas.gameObject, 5);
        PoolManager.CreatePool<Effect>("CFX2_EnemyDeathSkull", this.gameObject, 5);
        PoolManager.CreatePool<ThrowThing>("Ob_Enemy_Throw", this.gameObject, 5);
        PoolManager.CreatePool<Effect>("CFX_Hit_C White", this.gameObject, 5);
    }

    private void Update()
    {
        if (IsPlaying)
        {
            PlayTime += Time.deltaTime;
        }

        if (enemyListInStage.Count <= 0 && IsPlaying && !IsPotal)
        {
            //TODO : 게임 클리어
            IsPotal = true;
            if (stageManager.OnClearStage())
            {
                IsPlaying = false;
                uiGameClearPanel.PopUp(stageManager.nowStage);
            }
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
