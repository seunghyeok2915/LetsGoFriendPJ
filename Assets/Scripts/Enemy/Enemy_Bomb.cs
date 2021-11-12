using System.Collections;
using UnityEngine;

public class Enemy_Bomb : EnemyBase
{
    public EnemyFOV enemyFOV;
    public float bombDamage = 5f;//폭발대미지
    public float bombRange = 5f;

    private bool isAttacking;
    public override void Start()
    {
        base.Start();
        if (enemyFOV == null)
        {
            enemyFOV = GetComponent<EnemyFOV>();
        }

        if (moveAgent == null)
        {
            moveAgent = GetComponent<MoveAgent>();
        }
    }

    public override void StartEnemy() //적행동 시작
    {
        base.StartEnemy();
    }

    private void Update()
    {
        if (enemyFOV.IsTracePlayer() && enemyFOV.IsViewPlayer())
        {
            GameManager.Instance.IsCaught = true;
            if (!isAttacking)
            {
                moveAgent.Stop();
                StartCoroutine(AttackRoutine());
            }
        }
        else if (!isDead)
        {
            if (!isAttacking)
            {
                moveAgent.traceTarget = GameManager.Instance.GetPlayer().transform.position;
                enemyFOV.circularSectorMeshRenderer.gameObject.SetActive(false);
            }
        }

        animator.SetFloat("moveSpeed", moveAgent.speed);
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        Vector3 pos = GameManager.Instance.GetPlayer().transform.position;
        transform.LookAt(pos);
        yield return new WaitForSeconds(1f);
        Effect effect = PoolManager.GetItem<Effect>("CFX_Explosion");
        effect.transform.position = transform.position;
        if(Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().transform.position) < bombRange)
        {
            GameManager.Instance.GetPlayer().GetComponent<PlayerHealth>().OnDamage(bombDamage);
        }
        Die();
        yield return null;
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

    private float GetTotalDamage()
    {
        totalDamage = normalDamage;
        return totalDamage;
    }

    public override void Die()
    {
        base.Die();
        moveAgent.Stop();
        enemyFOV.circularSectorMeshRenderer.gameObject.SetActive(false);

    }
}
