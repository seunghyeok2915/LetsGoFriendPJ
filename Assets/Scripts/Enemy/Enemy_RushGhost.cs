using System.Collections;
using UnityEngine;

public class Enemy_RushGhost : EnemyBase
{
    public float dashDelay; // 몇초마다 대시할건지
    public float dashDelayRandom; // 랜덤추가
    public float dashSpeed; // 대시할때의 속도
    public float dashTime; // 몇초동안 대시할건지

    public float dashAdditionalDamage; // 대시할때 추가데미지 백분율 

    private Vector3 dashDir; // 대시할 방향
    private bool isDashing; // 대시 하고있는지 

    private float dashTimeCount;

    public override void Start()
    {
        base.Start();
    }

    public override void StartEnemy() //적행동 시작
    {
        base.StartEnemy();
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(dashDelay + Random.Range(0, dashDelayRandom));
            dashTimeCount = 0;
            dashDir = GetRandomDir();
            transform.LookAt(transform.position + dashDir);

            animator.SetTrigger(Attack);

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

    private void OnTriggerEnter(Collider other)
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
}
