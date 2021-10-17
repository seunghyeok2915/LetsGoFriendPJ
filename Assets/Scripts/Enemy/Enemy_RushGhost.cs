using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Enemy_RushGhost : Health
{
    public GameObject barCanvas;
    public GameObject hpBarObj;

    public float dashDelay; // 몇초마다 대시할건지
    public float dashDelayRandom; // 랜덤추가
    public float dashSpeed; // 대시할때의 속도
    public float dashTime; // 몇초동안 대시할건지

    public float dashAdditionalDamage; // 대시할때 추가데미지 백분율 
    public float normalDamage; // 기본 데미지

    private Vector3 dashDir; // 대시할 방향
    private bool isDashing; // 대시 하고있는지 
    private float totalDamage;

    private Animator animator;
    private SkinnedMeshRenderer[] materials;

    private EnemyHPBar enemyHpBar;

    private float dashTimeCount;
    private static readonly int Rush = Animator.StringToHash("Rush");
    private static readonly int DieName = Animator.StringToHash("Die");

    public override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        materials = GetComponentsInChildren<SkinnedMeshRenderer>();

        StartEnemy();
        SetHpBar();
    }
    public void StartEnemy() //적행동 시작
    {
        GameManager.Instance.AddEnemyInList(this.gameObject); //자신을 리스트에 추가함
        StartCoroutine(DashCoroutine());
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

    private IEnumerator DashCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(dashDelay + Random.Range(0, dashDelayRandom));
            dashTimeCount = 0;
            dashDir = GetRandomDir();
            transform.LookAt(transform.position + dashDir);

            animator.SetTrigger(Rush);

            while (true)
            {
                isDashing = true;
                dashTimeCount += Time.deltaTime;
                transform.position += dashDir * dashSpeed * Time.deltaTime;
                if (dashTimeCount > dashTime)
                {
                    isDashing = false;
                    break;
                }
                yield return new WaitForFixedUpdate();
            }

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isDead)
            {
                return; // 죽으면 공격안함
            }

            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.OnDamage(GetTotalDamage());
            }
        }
    }

    private Vector3 GetRandomDir() // 랜덤 방향 가져오기
    {
        Vector3 randDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        if (Physics.Raycast(transform.position, randDir, 2f)) //앞이 막혀있다면 반대로
        {
            return -randDir;
        }
        else
        {
            return randDir;
        }
    }

    private float GetTotalDamage()
    {
        totalDamage = isDashing ? normalDamage + normalDamage * (dashAdditionalDamage * 0.01f) : normalDamage; // 데미지 계산
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

        ShowDamagedEffect(); //피격 이펙트
        enemyHpBar.SetHPBar(MaxHealth, CurrentHealth); //HP바 업데이트
    }

    [ContextMenu("Revive")]
    public override void Revive()
    {
        if (!isDead)
        {
            return; //죽지않았다면 리턴
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
