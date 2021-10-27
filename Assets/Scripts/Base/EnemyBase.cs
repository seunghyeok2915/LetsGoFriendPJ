using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyBase : Health
{
    public GameObject barCanvas;
    public GameObject hpBarObj;

    public float normalDamage; // �⺻ ������
    protected float totalDamage; //���� ������ ���

    protected Animator animator;
    private SkinnedMeshRenderer[] materials;
    private EnemyHPBar enemyHpBar;

    protected static readonly int Attack = Animator.StringToHash("Attack");
    protected static readonly int DieName = Animator.StringToHash("Die");

    public virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        materials = GetComponentsInChildren<SkinnedMeshRenderer>();

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
        enemyHpBar.SetHPBar(MaxHealth, CurrentHealth);
        enemyHpBar.gameObject.SetActive(false);

        gameObject.SetActive(true);
        GameManager.Instance.AddEnemyInList(this.gameObject); //�ڽ��� ����Ʈ�� �߰���
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

    public void ShowDamagedEffect()
    {
        foreach (SkinnedMeshRenderer item in materials)
        {
            item.material.DOColor(Color.red, 0.2f).OnComplete(() => item.material.DOColor(Color.white, 0.2f));
        }
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        ShowDamagedEffect(); //�ǰ� ����Ʈ

        if (!isDead)
        {
            enemyHpBar.SetHPBar(MaxHealth, CurrentHealth); //HP�� ������Ʈ
            enemyHpBar.gameObject.SetActive(true);
        }
    }

    [ContextMenu("Revive")]
    public override void Revive()
    {
        if (!isDead)
        {
            return; //�����ʾҴٸ� ����
        }

        base.Revive();
        animator.SetTrigger("Revive");
        StartEnemy();
    }

    public override void Die()
    {
        base.Die();
        StopAllCoroutines();

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
