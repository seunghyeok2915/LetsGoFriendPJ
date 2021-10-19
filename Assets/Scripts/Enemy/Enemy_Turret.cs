using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Enemy_Turret : Health
{
    public GameObject barCanvas;
    public GameObject hpBarObj;

    public float shootDelay; // ���ʸ��� ����Ұ���
    public float shootDelayRandom; // �����߰�

    public float normalDamage; // �⺻ ������

    private Vector3 shootPos; // �� ��

    private float totalDamage; 

    private Animator animator;
    private SkinnedMeshRenderer[] materials;

    private EnemyHPBar enemyHpBar;

    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int DieName = Animator.StringToHash("Die");

    public override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        materials = GetComponentsInChildren<SkinnedMeshRenderer>();

        StartEnemy();
        SetHpBar();
    }
    public void StartEnemy() //���ൿ ����
    {
        GameManager.Instance.AddEnemyInList(this.gameObject); //�ڽ��� ����Ʈ�� �߰���
        StartCoroutine(ShootCoroutine());
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

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootDelay + Random.Range(0, shootDelayRandom));
            shootPos = GameManager.Instance.GetPlayer().transform.position;
            transform.LookAt(shootPos);

            animator.SetTrigger(Shoot);

            yield return new WaitForSeconds(0.2f);
            TurretBullet turretBullet = PoolManager.GetItem<TurretBullet>("TurretBullet");

            turretBullet.SetTurretBullet(transform.position, shootPos, 2f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isDead)
            {
                return; // ������ ���ݾ���
            }

            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.OnDamage(GetTotalDamage());
            }
        }
    }

    private Vector3 GetRandomDir() // ���� ���� ��������
    {
        Vector3 randDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        return randDir;
    }

    private float GetTotalDamage()
    {
        totalDamage = normalDamage; // ������ ���
        return totalDamage;
    }

    private void ShowDamagedEffect()
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
        enemyHpBar.SetHPBar(MaxHealth, CurrentHealth); //HP�� ������Ʈ
    }

    [ContextMenu("Revive")]
    public override void Revive()
    {
        if (!isDead)
        {
            return; //�����ʾҴٸ� ����
        }

        base.Revive();

        StartEnemy();

        animator.SetTrigger("Revive");
        enemyHpBar.SetHPBar(MaxHealth, CurrentHealth);

        gameObject.SetActive(true);
    }

    protected override void Die()
    {
        base.Die();

        animator.SetTrigger(DieName);

        StopAllCoroutines();

        GameManager.Instance.RemoveEnemyInList(this.gameObject);
        enemyHpBar.gameObject.SetActive(false);
    }
}
