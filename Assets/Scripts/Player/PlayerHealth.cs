public class PlayerHealth : Health
{
    public PlayerAnimationController playerAnimationController;
    public UIHealthORB playerHPBar;
    public PlayerHitEffect playerHitEffect;

    public override void Start()
    {
        base.Start();
        playerAnimationController = GetComponent<PlayerAnimationController>();

        playerHPBar.SetHPBar(MaxHealth, CurrentHealth);
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        playerHPBar.SetHPBar(MaxHealth, CurrentHealth);
        playerHitEffect.OnHitEffect();
    }

    protected override void Die()
    {
        base.Die();
        playerAnimationController.SetTrigger("Die");
    }

    public override void Revive() //부활
    {
        base.Revive();
        playerHPBar.SetHPBar(MaxHealth, CurrentHealth);

        playerAnimationController.SetTrigger("Revive");
    }
}
