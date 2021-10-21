using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour, IPoolable
{
    private Vector3 startPos;
    private Vector3 targetPos; // µµÂøÇÒ°÷  
    private float firingAngle = 30.0f;
    private float gravity = 9.8f;

    private float damage;

    public void SetTurretBullet(Vector3 startPos, Vector3 targetPos, float gravity,float firingAngle,float damage)
    {
        this.startPos = startPos;
        this.targetPos = targetPos;
        this.gravity = gravity;
        this.firingAngle = firingAngle;
        this.damage = damage;

        StartCoroutine(SimulateProjectile());
    }

    private IEnumerator SimulateProjectile()
    {
        // Move projectile to the position of throwing object + add some offset if needed.
        transform.position = startPos;

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
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.OnDamage(damage);
            }
                gameObject.SetActive(false);
        }
    }

    public void OnPool()
    {
        
    }
}
