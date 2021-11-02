using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour, IPoolable
{
    private ShurikenAttack shurikenAttack;
    private ShurikenMove shurikenMove;

    private void Awake()
    {
        shurikenMove = GetComponent<ShurikenMove>();
        shurikenAttack = GetComponent<ShurikenAttack>();
    }

    private void FixedUpdate()
    {
        shurikenMove.Move();
    }

    public void ShurikenMoveInit(Transform startPosition, Vector3 moveDir, float moveSpeed)
    {
        shurikenMove.ShurikenMoveInit(startPosition, moveDir, moveSpeed);
    }

    public void ShurikenAttackInit(float damage)
    {
        shurikenAttack.ShurikenAttackInit(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }

        shurikenAttack.OnAttack(other);

        if (GameManager.Instance.GetPlayer().GetComponent<PlayerStats>().CanUseSkill(ESkill.PierceShot))
        {
            return; //끄는 처리안함
        }

        if (other.CompareTag("Enemy"))
        { 

            gameObject.SetActive(false);
        }

    }

    public void OnPool()
    {

    }
}
