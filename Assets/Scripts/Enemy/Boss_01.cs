using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_01 : EnemyBase
{
    private Vector3 targetPos; // µµÂøÇÒ°÷  
    public float firingAngle = 70.0f;
    public float gravity = 9.8f;

    public float throwSpeed;
    public float throwDamage;

    public float angle;
    public int shootNum; //¸î°Ô ½ò°ÇÁö

    private Transform playerTrm;

    public override void Start()
    {
        base.Start();
        playerTrm = GameManager.Instance.GetPlayer().transform;
        StartCoroutine(BossRoutine());
    }

    private IEnumerator BossRoutine()
    {
        while (true)
        {
            float restTime = 2f;
            yield return new WaitForSeconds(restTime);
            int randPattern = Random.Range(0, 2);
            switch (randPattern)
            {
                case 0:
                    yield return ShootRandom(); //½î±â
                    break;
                case 1:
                    //Á¡ÇÁ 3¹ø
                    yield return JumpPattern();
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator JumpPattern()
    {
        for (int i = 0; i < 3; i++) //3¹ø
        {
            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger("Jump");
            yield return SimulateProjectile();

            float degree = 45f;

            for (int j = 0; j < 8; j++)
            {
                Vector3 rotVec = new Vector3(0, degree * j, 0);
                ThrowThing throwThing = PoolManager.GetItem<ThrowThing>("Ob_Enemy_Throw");
                throwThing.transform.position = transform.position + new Vector3(0, 0.5f, 0);
                Vector3 pos = playerTrm.position;
                Vector3 newPos = Quaternion.Euler(rotVec) * pos;
                throwThing.SetData(throwSpeed, throwDamage, newPos + transform.position);
            }
        }
    }

    private IEnumerator SimulateProjectile()
    {
        // Move projectile to the position of throwing object + add some offset if needed.
        targetPos = playerTrm.position;

        // Calculate distance to target
        float target_Distance = Vector3.Distance(transform.position, targetPos);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        transform.rotation = Quaternion.LookRotation(targetPos - transform.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }

        BulletHitGroundEffect bullet = PoolManager.GetItem<BulletHitGroundEffect>("BulletHitGroundEffect");
        bullet.transform.position = transform.position;
    }

    private IEnumerator ShootRandom()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < shootNum; i++)
        {
            transform.LookAt(playerTrm);
            float startAngle = angle - transform.eulerAngles.y;
            float theta = startAngle + Random.Range(0f, 60f);
            theta *= Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta));

            ThrowThing throwThing = PoolManager.GetItem<ThrowThing>("Ob_Enemy_Throw");
            throwThing.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            throwThing.SetData(throwSpeed, throwDamage, dir, true);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
