using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyBase : Health
{
    public float exp;
    public GameObject barCanvas;
    public GameObject hpBarObj;

    public float normalDamage; // �⺻ ������
    protected float totalDamage; //���� ������ ���

    protected Animator animator;
    private SkinnedMeshRenderer[] materials;
    private EnemyHPBar enemyHpBar;
    protected MoveAgent moveAgent;
    private BoxCollider boxCollider;

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
        boxCollider = GetComponent<BoxCollider>();

        SetHP();
        SetHpBar();
        StartEnemy();
    }

    public virtual void StartEnemy() //���ൿ ����
    {
        foreach (SkinnedMeshRenderer item in materials)
        {
            item.material.color = Color.white;

        }
        boxCollider.enabled = true;
        enemyHpBar.SetHPBar(MaxHealth, CurrentHealth);
        enemyHpBar.gameObject.SetActive(false);

        gameObject.SetActive(true);
        GameManager.Instance.AddEnemyInList(gameObject); //�ڽ��� ����Ʈ�� �߰���
    }

    public void SetHpBar() //HP �� ����, �ʱ�ȭ
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
        InvokeRepeating(nameof(StartShake), 0f, 0.005f);
        Invoke(nameof(StopShake), duration);
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
        CancelInvoke(nameof(StartShake));
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);


        if (GameManager.Instance.GetPlayer().CanUseSkill(ESkill.FireDotD) && !isFire)
        {
            isFire = true;
            StartCoroutine(FireDotsDamage(5f, damage));
            //��Ʈ������ �Ծ����
        }

        if (GameManager.Instance.GetPlayer().CanUseSkill(ESkill.Ice) && !isIce)
        {
            isIce = true;
            StartCoroutine(Ice(5f));
            //��Ʈ������ �Ծ����
        }
        ShowDamagedEffect(damage); //�ǰ� ����Ʈ

        if (!isDead)
        {
            enemyHpBar.SetHPBar(MaxHealth, CurrentHealth); //HP�� ������Ʈ
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
        if (moveAgent == null)
        {
            yield break;
        }

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
        if (isDead)
        {
            base.Revive();
            animator.SetTrigger("Revive");
            StartEnemy();
        }
    }

    protected override void Die()
    {
        base.Die();
        StopAllCoroutines();
        boxCollider.enabled = false;

        if (GameManager.Instance.GetPlayer().CanUseSkill(ESkill.DrainHealth))
        {
            //���� �ؾ���
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

        float cubeExp = 5f;
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
