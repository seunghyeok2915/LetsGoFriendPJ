using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHp;
    public float currentHp;

    public bool isDead = false;

    public UnityEvent onDead;
    public UnityEvent onDamage;

    public virtual void Start()
    {
        currentHp = maxHp;
    }

    public virtual void OnDamage(float damage)
    {
        if (isDead) return;

        currentHp -= damage;
        onDamage?.Invoke();
        Debug.Log(gameObject.name + " 이 " + damage + " 만큼 피해를 입었습니다");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        onDead?.Invoke();

        isDead = true;
    }
}
