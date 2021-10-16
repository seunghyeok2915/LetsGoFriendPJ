using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public PlayerAnimationController playerAnimationController;
    public UIHealthORB playerHPBar;

    public override void Start()
    {
        base.Start();
        playerAnimationController = GetComponent<PlayerAnimationController>();

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
        playerAnimationController.PlayDeathAnimation();
    }

    public void Revive() //부활
    {
        isDead = false;

        currentHp = maxHp;
        playerHPBar.SetHPBar(maxHp, currentHp);

        playerAnimationController.OnRevivePlayer();
    }
}
