using UnityEngine;

public class Enemy_Trace_01 : EnemyBase
{
    public EnemyFOV enemyFOV;
    public override void Start()
    {
        base.Start();
        if (enemyFOV == null)
        {
            enemyFOV = GetComponent<EnemyFOV>();
        }
    }

    public override void StartEnemy() //적행동 시작
    {
        base.StartEnemy();
    }

    private void Update()
    {
        if(enemyFOV.IsTracePlayer() && enemyFOV.IsViewPlayer())
        {
            print("감지됨");
            GameManager.Instance.IsCaught = true;
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

    private float GetTotalDamage()
    {
        totalDamage = normalDamage;
        return totalDamage;
    }
}
