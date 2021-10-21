using System.Collections;
using UnityEngine;

public class Enemy_Turret : EnemyBase
{
    public float shootDelay; // 몇초마다 대시할건지
    public float shootDelayRandom; // 랜덤추가

    public float bulletGravity; //총알 중력 높을수록 빠름
    public float firingAngle; //총알 날리는 각도

    private Vector3 shootPos; // 쏠 곳

    public override void Start()
    {
        base.Start();
    }

    public override void StartEnemy() //적행동 시작
    {
        base.StartEnemy();
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootDelay + Random.Range(0, shootDelayRandom));
            shootPos = GameManager.Instance.GetPlayer().transform.position;
            transform.LookAt(shootPos);

            animator.SetTrigger(Attack);

            yield return new WaitForSeconds(0.2f);
            TurretBullet turretBullet = PoolManager.GetItem<TurretBullet>("TurretBullet");

            turretBullet.SetTurretBullet(transform.position, shootPos, bulletGravity, firingAngle, normalDamage);
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

    private float GetTotalDamage()
    {
        totalDamage = normalDamage; // 데미지 계산
        return totalDamage;
    }
}
