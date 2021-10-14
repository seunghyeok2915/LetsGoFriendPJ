using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public GameObject barCanvas;
    public GameObject playerHPBarObj;

    private PlayerHPBar playerHPBar;

    public override void Start()
    {
        base.Start();

        playerHPBar = Instantiate(playerHPBarObj, transform.position, Quaternion.identity, barCanvas.transform).GetComponent<PlayerHPBar>();
        //playerHPBar.transform.parent = barCanvas.transform;
        playerHPBar.transform.localRotation = Quaternion.Euler(Vector3.zero);
        playerHPBar.Init(gameObject.transform);

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
