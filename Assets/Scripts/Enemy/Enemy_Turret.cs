using System.Collections;
using UnityEngine;

public class Enemy_Turret : EnemyBase
{
    public float shootDelay; // ���ʸ��� ����Ұ���
    public float shootDelayRandom; // �����߰�

    public float bulletGravity; //�Ѿ� �߷� �������� ����
    public float firingAngle; //�Ѿ� ������ ����

    private Vector3 shootPos; // �� ��

    public override void Start()
    {
        base.Start();
    }

    public override void StartEnemy() //���ൿ ����
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
                return; // ������ ���ݾ���
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
        totalDamage = normalDamage; // ������ ���
        return totalDamage;
    }
}
