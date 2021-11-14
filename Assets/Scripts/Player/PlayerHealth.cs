using UnityEngine;

public class PlayerHealth : Health
{
    public PlayerAnimationController playerAnimationController;
    public UIHealthORB playerHPBar;

    public GameObject barCanvas;
    public GameObject playerHPBarObj;

    private PlayerHPBar playerHPBarOnPlayer;

    public PlayerHitEffect playerHitEffect;

    public void Start()
    {
        SetHP();
        Bind();
        HpBarsSetting();
    }

    private void Bind()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    public override void IncreaseMaxHp(float hp)
    {
        base.IncreaseMaxHp(hp);
        Heal(hp * CurrentHealth * 0.01f);
        UpdateHPUI();
    }

    private void HpBarsSetting()
    {
        //플레이어 위 HP바 생성
        playerHPBarOnPlayer = Instantiate(playerHPBarObj, transform.position, Quaternion.identity, barCanvas.transform).GetComponent<PlayerHPBar>();
        playerHPBarOnPlayer.transform.localRotation = Quaternion.Euler(Vector3.zero);
        playerHPBarOnPlayer.Init(gameObject.transform);

        UpdateHPUI();
    }

    private void UpdateHPUI()
    {
        playerHPBar.SetHPBar(MaxHealth, CurrentHealth);
        playerHPBarOnPlayer.SetHPBar(MaxHealth, CurrentHealth);
    }

    public override void Heal(float healAmount)
    {
        base.Heal(healAmount);
        UpdateHPUI();
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        UpdateHPUI();

        playerHitEffect.OnHitEffect();
    }

    protected override void Die()
    {
        base.Die();
        GameManager.Instance.EndGame();
        playerAnimationController.SetTrigger("Die");
    }

    public override void Revive() //부활
    {
        base.Revive();

        UpdateHPUI();

        playerAnimationController.SetTrigger("Revive");
    }
}
