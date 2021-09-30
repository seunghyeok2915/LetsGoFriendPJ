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

    private void Start()
    {
        currentHp = maxHp;
    }

    public void OnDamage(float damage)
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

    private void Die()
    {
        if (isDead) return;
        onDead?.Invoke();

        isDead = true;
    }
}
