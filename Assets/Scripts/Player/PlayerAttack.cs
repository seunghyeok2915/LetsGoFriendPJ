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
    private Transform targetTrm;
    private float tempAttackTimer;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    private void Start()
    {
        if (attackDelay != 1)
        {
            var attackSpeed = 1 / attackDelay;
            playerAnimationController.SetAttackAnimSpeed(attackSpeed);
        }
    }

    private void Update()
    {
        tempAttackTimer += Time.deltaTime;

        if (AttackCheck())
        {
            PlayerAttackAnimation(); //어택
            transform.LookAt(targetTrm);
            tempAttackTimer = 0;
        }
    }

    private bool AttackCheck()
    {
        if (!playerInput.IsMoving)
        {
            var enemyList = GameManager.Instance.GetEnemyListInStage();
            if (enemyList.Count > 0)
            {
                nowTarget = FindNearestEnemy();

                if (nowTarget != null)
                {
                    targetTrm = nowTarget.transform;
                    if (tempAttackTimer > attackDelay)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private GameObject FindNearestEnemy()
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        var enemys = GameManager.Instance.GetEnemyListInStage();

        var neareastObject = enemys // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
            .OrderBy(obj =>
        {
            return Vector3.Distance(transform.position, obj.transform.position);
        })
        .FirstOrDefault(x => (transform.position - x.transform.position).sqrMagnitude < attackRange * attackRange);

        return neareastObject;
    }

    private void PlayerAttackAnimation()
    {
        playerAnimationController.PlayAttackAnimation();
    }

    public void ThrowShuriken()
    {
        if (playerInput.IsMoving) return;
        Shuriken shuriken = PoolManager.GetItem<Shuriken>("Shuriken1");

        shuriken.ShurikenMoveInit(throwPos, targetTrm.position - transform.position, shurikenSpeed);
        shuriken.ShurikenAttackInit(attackDamage);
    }
}
