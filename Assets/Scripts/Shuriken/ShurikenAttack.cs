using UnityEngine;

public class ShurikenAttack : MonoBehaviour
{
    public float damage;

    public void ShurikenAttackInit(float damage)
    {
        this.damage = damage;
    }

    public void OnAttack(Collider other)
    {
        if (other.CompareTag("Enemy")) //데미지 처리
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                if (health.isDead)
                {
                    return;
                }

                health.OnDamage(damage);
                Debug.Log(other + " 에게 " + damage + " 만큼의 데미지를 주었습니다.");
            }
        }
    }
}
