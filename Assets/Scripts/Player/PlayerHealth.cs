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

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        UpdateHPUI();

        playerHitEffect.OnHitEffect();
    }

    public override void Die()
    {
        base.Die();
        playerAnimationController.SetTrigger("Die");
    }

    public override void Revive() //부활
    {
        base.Revive();

        UpdateHPUI();

        playerAnimationController.SetTrigger("Revive");
    }
}
