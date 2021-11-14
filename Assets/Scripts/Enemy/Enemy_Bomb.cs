using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Enemy_Bomb : EnemyBase
{
    public EnemyFOV enemyFOV;
    public float bombDamage = 5f;//폭발대미지
    public float bombRange = 5f;
    public GameObject bombRangeShow;
    private bool isAttacking;

    private PlayerStats playerStats;
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
        
        playerStats = GameManager.Instance.GetPlayer();
    }

    public override void StartEnemy() //적행동 시작
    {
        base.StartEnemy();
    }

    private void Update()
    {
        if(isDead) return;

        if (!enemyFOV.IsTracePlayer() || !enemyFOV.IsViewPlayer())
        {
            if (!isAttacking)
            {
                moveAgent.traceTarget = playerStats.transform.position;
                enemyFOV.circularSectorMeshRenderer.gameObject.SetActive(false);
            }
            else
            {
                GameManager.Instance.IsCaught = true;
                
                if (!isAttacking)
                {
                    moveAgent.Stop();
                    StartCoroutine(AttackRoutine());
                }
            }
        }

        animator.SetFloat("moveSpeed", moveAgent.speed);
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        Vector3 pos = playerStats.transform.position;
        transform.LookAt(pos);
        bombRangeShow.transform.DOScale(new Vector3(5, 5, 5), 1f);
        yield return new WaitForSeconds(1f);
        Effect effect = PoolManager.GetItem<Effect>("CFX_Explosion");
        effect.transform.position = transform.position;
        if(Vector3.Distance(transform.position, playerStats.transform.position) < bombRange)
        {
            playerStats.playerHealth.OnDamage(bombDamage);
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

    protected override void Die()
    {
        base.Die();
        moveAgent.Stop();
        bombRangeShow.SetActive(false);
        enemyFOV.circularSectorMeshRenderer.gameObject.SetActive(false);
    }
}
