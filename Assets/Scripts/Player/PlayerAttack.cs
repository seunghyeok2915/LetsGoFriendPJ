using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{
    [Header("인스펙터 세팅")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private Transform throwPos;
    [SerializeField] private LayerMask enemyLayer;
    [Header("플레이어 세팅")]
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackRange;
    [SerializeField] private float shurikenSpeed;

    private GameObject nowTarget;
    private float tempAttackTimer;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
        tempAttackTimer += Time.deltaTime;

        if (AttackCheck())
        {
            PlayerAttackAnimation(); //어택
            transform.LookAt(nowTarget.transform);
            tempAttackTimer = 0;
        }
    }

    private bool AttackCheck()
    {
        if (!playerInput.IsMoving)
        {
            if (GameManager.Instance.enemyListInStage.Count > 0)
            {
                nowTarget = GameManager.Instance.enemyListInStage.OrderBy(x => transform.position - x.transform.position)
                    .FirstOrDefault(x => (transform.position - x.transform.position).sqrMagnitude < attackRange * attackRange);

                if (nowTarget != null)
                {
                    if (tempAttackTimer > attackDelay)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void PlayerAttackAnimation()
    {
        playerAnimationController.PlayAttackAnimation();
    }

    public void ThrowShuriken()
    {
        Shuriken shuriken = PoolManager.GetItem<Shuriken>("Shuriken1");

        shuriken.ShurikenMoveInit(throwPos, nowTarget.transform.position - transform.position, shurikenSpeed);
        shuriken.ShurikenAttackInit(attackDamage);
    }
}
