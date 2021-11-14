using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth { get; set; }
    public bool Invincible { get; set; }

    public bool isDead;

    protected void SetHP()
    {
        CurrentHealth = MaxHealth;
    }

    public virtual void OnDamage(float damage)
    {
        if (Invincible) return;
        if (isDead) return;

        float healthBefore = CurrentHealth;
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        // call OnDamage action
        float trueDamageAmount = healthBefore - CurrentHealth;

        if (trueDamageAmount > 0f) //데미지가 들어갔는지
            Debug.Log(gameObject.name + " 이 " + damage + " 만큼 피해를 입었습니다");


        if (CurrentHealth <= 0) Die();
    }

    public virtual void IncreaseMaxHp(float hp)
    {
        MaxHealth += hp;
    }

    public virtual void Heal(float healAmount)
    {
        float healthBefore = CurrentHealth;
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);
    }

    public virtual void Revive() //부활
    {
        isDead = false;
        CurrentHealth = MaxHealth;
    }

    public virtual void Kill()
    {
        CurrentHealth = 1;
        OnDamage(1);
    }

    protected virtual void Die()
    {
        if (!isDead) isDead = true;
    }
}