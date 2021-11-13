using System.Collections;
using UnityEngine;

public class Enemy_Radial : EnemyBase
{
    public EnemyFOV enemyFOV;

    public float attackInterval = 2; //쏘고 기다리는 시간
    public float attackDelay = 1.5f; //쏘기까지 기다리는시간

    public float throwSpeed;
    public float throwDamage;

    public float maxDist;

    private float attackIntervalTimer = 0;
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
            if (!isAttacking && Time.time - attackIntervalTimer > attackInterval)
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
        float time = Time.time;

        Vector3 pos = GameManager.Instance.GetPlayer().transform.position;
        transform.LookAt(pos);
        float degree = 45f;


        //라인 렌더러 그려줘야해
        while (true)
        {
            for (int i = 0; i < 8; i++)
            {
                Vector3 rotVec = new Vector3(0, degree * i, 0);
                Vector3 newPos = Quaternion.Euler(rotVec) * pos;
                yield return new WaitForSeconds(0.1f);
            }

            if (Time.time - time > attackDelay)
            {
                break;
            }

            yield return null;
        }

        animator.SetTrigger("Attack");

        for (int i = 0; i < 8; i++)
        {
            Vector3 rotVec = new Vector3(0, degree * i, 0);
            ThrowThing throwThing = PoolManager.GetItem<ThrowThing>("Ob_Enemy_Throw");
            throwThing.transform.position = transform.position + new Vector3(0, 0.5f, 0);

            //var newPos = new Vector3(pos.x * Mathf.Cos(i * degree) - pos.y * Mathf.Sin(i * degree), 0, pos.x * Mathf.Sin(i * degree) - pos.y * Mathf.Cos(i * degree));
            Vector3 newPos = Quaternion.Euler(rotVec) * pos;
            throwThing.SetData(throwSpeed, throwDamage, newPos + transform.position);
        }

        //발사


        isAttacking = false;
        attackIntervalTimer = Time.time;
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
