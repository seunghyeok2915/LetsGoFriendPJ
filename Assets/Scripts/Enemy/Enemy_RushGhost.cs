using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy_RushGhost : Health
{
    public GameObject barCanvas;
    public GameObject hpBarObj;

    public float dashDelay; // 몇초마다 대시할건지
    public float dashSpeed; // 대시할때의 속도
    public float dashTime; // 몇초동안 대시할건지

    public float dashAdditionalDamage; // 대시할때 추가데미지 백분율 
    public float normalDamage; // 기본 데미지

    private Vector3 dashDir; // 대시할 방향
    private bool isDashing; // 대시 하고있는지 
    private float totalDamage;

    private Animator animator;
    private Health health;
    private SkinnedMeshRenderer[] materials; //테스트

    private EnemyHPBar enemyHPBar;

    private float dashTimeCount;

    public override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        materials = GetComponentsInChildren<SkinnedMeshRenderer>();

        if (barCanvas == null)
        {
            barCanvas = GameObject.Find("HPBarCanvas");
        }

        GameManager.Instance.AddEnemyInList(this.gameObject);

        enemyHPBar = Instantiate(hpBarObj, transform.position, Quaternion.identity, barCanvas.transform).GetComponent<EnemyHPBar>();

        enemyHPBar.transform.localRotation = Quaternion.Euler(Vector3.zero);
        enemyHPBar.Init(gameObject.transform);

        enemyHPBar.SetHPBar(maxHp, currentHp);

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        while (true)
        {
            dashTimeCount = 0;
            dashDir = GetRandomDir();
            transform.LookAt(transform.position + dashDir);

            animator.SetTrigger("Rush");

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
            yield return new WaitForSeconds(dashDelay);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
                health.OnDamage(GetTotalDamage());
        }
    }

    private Vector3 GetRandomDir() // 랜덤 방향 가져오기
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    private float GetTotalDamage()
    {
        totalDamage = isDashing ? normalDamage + normalDamage * (dashAdditionalDamage * 0.01f) : normalDamage; // 데미지 계산
        return totalDamage;
    }

    private void ShowDamagedEffect()
    {
        foreach (var item in materials)
        {
            item.material.DOColor(Color.red, 0.2f).OnComplete(() => item.material.DOColor(Color.white, 0.2f));
        }
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        ShowDamagedEffect(); //피격 이펙트
        enemyHPBar.SetHPBar(maxHp, currentHp); //HP바 업데이트
    }

    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("Die");

        StopAllCoroutines();

        enemyHPBar.gameObject.SetActive(false);
        GameManager.Instance.RemoveEnemyInList(this.gameObject);
    }
}
