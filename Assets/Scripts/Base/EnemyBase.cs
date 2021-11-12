using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyBase : Health
{
    public float exp;
    public GameObject barCanvas;
    public GameObject hpBarObj;

    public bool canBePush = true; //밀릴수있는가
    private const float pushForce = 3.0f;

    public float normalDamage; // 기본 데미지
    protected float totalDamage; //총합 데미지 계산

    protected Animator animator;
    private SkinnedMeshRenderer[] materials;
    private EnemyHPBar enemyHpBar;
    protected MoveAgent moveAgent;

    private GameObject fireFx;
    private bool isFire;
    private bool isIce;

    protected static readonly int Attack = Animator.StringToHash("Attack");
    protected static readonly int DieName = Animator.StringToHash("Die");

    float shakeRange;
    float duration;
    Vector3 shakePos;

    public virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        materials = GetComponentsInChildren<SkinnedMeshRenderer>();
        moveAgent = GetComponent<MoveAgent>();

        SetHP();
        SetHpBar();
        StartEnemy();
    }

    public virtual void StartEnemy() //적행동 시작
    {
        foreach (SkinnedMeshRenderer item in materials)
        {
            item.material.color = Color.white;

        }
        enemyHpBar.SetHPBar(MaxHealth, CurrentHealth);
        enemyHpBar.gameObject.SetActive(false);

        gameObject.SetActive(true);
        GameManager.Instance.AddEnemyInList(this.gameObject); //자신을 리스트에 추가함
    }

    public void SetHpBar() //HP 바 생성, 초기화
    {
        if (barCanvas == null)
        {
            barCanvas = GameObject.Find("HPBarCanvas");
        }

        enemyHpBar = Instantiate(hpBarObj, transform.position, Quaternion.identity, barCanvas.transform).GetComponent<EnemyHPBar>();

        enemyHpBar.transform.localRotation = Quaternion.Euler(Vector3.zero);
        enemyHpBar.Init(gameObject.transform);

        enemyHpBar.SetHPBar(MaxHealth, CurrentHealth);
    }

    public void ShowDamagedEffect(float damage)
    {
        Shake(0.1f, 0.05f);
        if (!isIce)
        {
            foreach (SkinnedMeshRenderer item in materials)
            {
                item.material.DOColor(Color.red, 0.2f).OnComplete(() => item.material.DOColor(Color.white, 0.2f));
            }
        }


        Effect effect = PoolManager.GetItem<Effect>("CFX_Hit_C White");
        effect.transform.position = transform.position;

        PopupDamage popup = PoolManager.GetItem<PopupDamage>("PopupDamage");
        popup.SetData(damage, transform);
    }

    public void Shake(float shakeRange, float duration)
    {
        this.shakeRange = shakeRange;
        this.duration = duration;

        shakePos = transform.position;
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }

    void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos = transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        transform.position = cameraPos;
    }

    void StopShake()
    {
        transform.position = shakePos;
        CancelInvoke("StartShake");
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);


        if (GameManager.Instance.GetPlayer().CanUseSkill(ESkill.FireDotD) && !isFire)
        {
            isFire = true;
            StartCoroutine(FireDotsDamage(5f, damage));
            //도트데미지 입어야함
        }

        if (GameManager.Instance.GetPlayer().CanUseSkill(ESkill.Ice) && !isIce)
        {
            isIce = true;
            StartCoroutine(Ice(5f));
            //도트데미지 입어야함
        }
        ShowDamagedEffect(damage); //피격 이펙트

        if (!isDead)
        {
            enemyHpBar.SetHPBar(MaxHealth, CurrentHealth); //HP바 업데이트
            enemyHpBar.gameObject.SetActive(true);
        }
    }

    private IEnumerator FireDotsDamage(float time, float damage)
    {
        fireFx = PoolManager.GetItem<Effect>("CFX4 Fire").gameObject;
        fireFx.transform.parent = transform;
        fireFx.transform.localPosition = Vector3.zero;

        while (time > 0)
        {
            OnDamage(damage / 5);
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }

        fireFx.SetActive(false);
        isFire = false;
    }

    private IEnumerator Ice(float time)
    {
        float oriSpeed = moveAgent.traceSpeed;
        moveAgent.traceSpeed = oriSpeed / 2f;
        while (time > 0)
        {
            foreach (SkinnedMeshRenderer item in materials)
            {
                item.material.color = Color.blue;
            }
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }
        foreach (SkinnedMeshRenderer item in materials)
        {
            item.material.color = Color.white;
        }
        moveAgent.traceSpeed = oriSpeed;

        isIce = false;
    }

    [ContextMenu("Revive")]
    public override void Revive()
    {
        if (!isDead)
        {
            return; //죽지않았다면 리턴
        }

        base.Revive();
        animator.SetTrigger("Revive");
        StartEnemy();
    }

    public override void Die()
    {
        base.Die();
        StopAllCoroutines();

        if (GameManager.Instance.GetPlayer().CanUseSkill(ESkill.DrainHealth))
        {
            //피흡 해야함
            GameManager.Instance.GetPlayer().DrainHealth();
        }

        CameraManager.Instance.Shake(0.25f, 0.1f);
        Effect effect = PoolManager.GetItem<Effect>("CFX2_EnemyDeathSkull");
        effect.transform.position = transform.position;

        if (fireFx != null)
        {
            fireFx.SetActive(false);
        }

        isFire = false;

        float cubeExp = 1f;
        float expCnt = exp / cubeExp;

        for (int i = 0; i < expCnt; i++)
        {
            ExpCube expCube = PoolManager.GetItem<ExpCube>("ExpCube");
            expCube.SetData(cubeExp, transform);
        }

        //GameManager.Instance.GetPlayer().AddExp(exp);
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        animator.SetTrigger(DieName);

        foreach (SkinnedMeshRenderer item in materials)
        {
            item.material.DOColor(Color.black, 1.5f);
        }

        GameManager.Instance.RemoveEnemyInList(this.gameObject);
        enemyHpBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

    public void AssassinateEnemy()
    {
        GetComponent<MoveAgent>().Stop();
        StartCoroutine(Assassinate());
    }

    private IEnumerator Assassinate()
    {
        const float animTime = 3f;
        animator.SetTrigger("Assassinate");
        yield return new WaitForSeconds(animTime);
        Die();
    }
}
