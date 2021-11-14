using System.Collections;
using UnityEngine;

public class Enemy_Sniper_02 : EnemyBase
{
    public EnemyFOV enemyFOV;

    public float attackInterval = 2; //쏘고 기다리는 시간
    public float attackDelay = 1.5f; //쏘기까지 기다리는시간

    public float throwSpeed;
    public float throwDamage;

    public float maxDist;

    private float attackIntervalTimer;
    private LineRenderer lineRenderer;
    private bool isAttacking;

    public override void Start()
    {
        base.Start();
        if (enemyFOV == null) enemyFOV = GetComponent<EnemyFOV>();

        if (moveAgent == null) moveAgent = GetComponent<MoveAgent>();

        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.endColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
    }

    public void DrawDangerLine()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * maxDist);
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

        var pos = GameManager.Instance.GetPlayer().transform.position;
        transform.LookAt(pos);
        DrawDangerLine();

        //라인 렌더러 그려줘야해
        while (true)
        {
            if (Time.time - time > attackDelay) break;
            yield return null;
        }

        animator.SetTrigger("Attack");
        var throwThing = PoolManager.GetItem<ThrowThing>("Ob_Enemy_Throw");
        throwThing.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        throwThing.SetData(throwSpeed, throwDamage, pos);

        //발사


        isAttacking = false;
        lineRenderer.enabled = false;
        attackIntervalTimer = Time.time;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isDead) return; // 죽으면 공격안함

            var health = other.gameObject.GetComponent<Health>();
            if (health != null) health.OnDamage(GetTotalDamage());
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
        lineRenderer.enabled = false;
        moveAgent.Stop();
        enemyFOV.circularSectorMeshRenderer.gameObject.SetActive(false);
    }
}