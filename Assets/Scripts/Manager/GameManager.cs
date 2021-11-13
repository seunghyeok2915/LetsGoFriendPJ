using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

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
        StageManager = GetComponent<StageManager>();
    }

    protected virtual void OnDestroy()
    {
        _instance = default;
    }

    public UIGameClearPanel uiGameClearPanel;
    public Canvas popupCanvas;
    public Image fadeImage;
    public Animator clearAnim;

    private List<GameObject> enemyListInStage = new List<GameObject>();
    private PlayerStats player;
    private StageManager stageManager;
    private bool isCaught = false;
    private bool isPlaying = true;
    private float playTime = 0f;
    private int earnZem = 0;


    public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
    public bool IsCaught { get => isCaught; set => isCaught = value; }
    public float PlayTime { get => playTime; set => playTime = value; }
    public int EarnZem { get => earnZem; set => earnZem = value; }
    public StageManager StageManager { get => stageManager; set => stageManager = value; }

    private void Start()
    {
        FadeOut();

        PoolManager.CreatePool<Shuriken>("Shuriken1", this.gameObject, 5);
        PoolManager.CreatePool<TurretBullet>("TurretBullet", this.gameObject, 5);
        PoolManager.CreatePool<BulletHitGroundEffect>("BulletHitGroundEffect", this.gameObject, 5);
        PoolManager.CreatePool<Effect>("CFX4 Fire", this.gameObject, 5);
        PoolManager.CreatePool<ExpCube>("ExpCube", this.gameObject, 20);
        PoolManager.CreatePool<PopupDamage>("PopupDamage", popupCanvas.gameObject, 5);
        PoolManager.CreatePool<Effect>("CFX2_EnemyDeathSkull", this.gameObject, 5);
        PoolManager.CreatePool<ThrowThing>("Ob_Enemy_Throw", this.gameObject, 5);
        PoolManager.CreatePool<Effect>("CFX_Hit_C White", this.gameObject, 5);
        PoolManager.CreatePool<Effect>("CFX_Explosion", this.gameObject, 5);
    }

    private void Update()
    {
        if (IsPlaying)
        {
            PlayTime += Time.deltaTime;
        }

    }

    public void EndGame()
    {
        IsPlaying = false;
        uiGameClearPanel.PopUp();
    }

    private bool CheckEnd()
    {
        if (enemyListInStage.Count <= 0 && IsPlaying)
        {
            //TODO : 게임 클리어 우승
            if (StageManager.OnClearStage())
            {
                EndGame();
                return true;
            }
            clearAnim.SetTrigger("Clear");
        }
        return false;
    }

    public void FadeIn()
    {
        fadeImage.DOFade(0f, 0.01f);
        fadeImage.DOFade(1f, 0.5f);
    }

    public void FadeOut()
    {
        fadeImage.DOFade(1f, 0.01f);
        fadeImage.DOFade(0f, 1);
    }

    public void FadeInOut(UnityAction callback)
    {
        fadeImage.DOFade(0f, 0.01f);
        fadeImage.DOFade(1f, 0.5f).OnComplete(() =>
        {
            callback?.Invoke();
            fadeImage.DOFade(1f, 0.01f);
            fadeImage.DOFade(0, 1f);
        });
    }




    public void AddEnemyInList(GameObject enemy)
    {
        enemyListInStage.Add(enemy);
    }

    public void RemoveEnemyInList(GameObject enemy)
    {
        enemyListInStage.Remove(enemy);
        CheckEnd();
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
