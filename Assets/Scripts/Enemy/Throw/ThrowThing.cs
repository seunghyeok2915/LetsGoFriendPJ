using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowThing : MonoBehaviour
{
    private float speed;
    private float damage;

    private Vector3 moveDir = Vector3.zero;

    public void SetData(float speed,float damage,Vector3 targetPos)
    {
        this.speed = speed;
        this.damage = damage;

        moveDir = (targetPos - transform.position).normalized;
        moveDir.y = 0;
    }

    public void FixedUpdate()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.OnDamage(damage);
            }
        }
        if(collision.gameObject.CompareTag("Wall"))
        {
        gameObject.SetActive(false);
        }
    }
}
