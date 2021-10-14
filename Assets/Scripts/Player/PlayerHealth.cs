using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public UIHealthORB playerHPBar;

    public override void Start()
    {
        base.Start();
        playerHPBar.SetHPBar(maxHp, currentHp);
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        playerHPBar.SetHPBar(maxHp, currentHp);
    }

    protected override void Die()
    {
        base.Die();
    }
}
