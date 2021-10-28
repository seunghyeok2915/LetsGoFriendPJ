using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("인스펙터 세팅")]
    [SerializeField] private PlayerInput playerInput; //플레이어 인풋 관리
    [SerializeField] private PlayerAnimationController playerAnimationController; //플레이어 애니메이션 관리
    [SerializeField] private PlayerHealth playerHealth; //플레이어 체력관리
    [SerializeField] private Transform throwPos; // 표창 나갈곳
    [Header("플레이어 세팅")]
    [SerializeField] private float attackDamage; //공격 데미지
    [SerializeField] private float attackDelay; // 몇초마다 공격할건지
    [SerializeField] private float attackRange; // 공격 거리
    [SerializeField] private float assassinateRange; //  거리
    [SerializeField] private float shurikenSpeed; // 표창 속도

    private GameObject nowTarget;
    private Transform targetTrm;
    private float tempAttackTimer;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        SetAnimationSpeed();
    }

    public bool CanAssassinate()
    {
        nowTarget = FindNearestEnemy();
        if (nowTarget != null)
        {
            if (Vector3.Distance(transform.position, nowTarget.transform.position) < assassinateRange)
            {
                return true;
            }
        }
        return false;
    }

    private void SetAnimationSpeed()
    {
        if (attackDelay != 1)
        {
            float attackSpeed = 1 / attackDelay;
            playerAnimationController.SetAttackAnimSpeed(attackSpeed);
        }
    }

    private void Update()
    {
        if (playerHealth.isDead || !GameManager.Instance.IsCaught)
        {
            return;//죽었을땐 공격안함
        }

        tempAttackTimer += Time.deltaTime;

        if (AttackCheck())
        {
            PlayerAttackAnimation(); //어택
            tempAttackTimer = 0;
        }
    }

    private bool AttackCheck() //공격 가능한지 상태체크
    {
        if (!playerInput.IsMoving)
        {
            List<GameObject> enemyList = GameManager.Instance.GetEnemyListInStage();
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

    private GameObject FindNearestEnemy() //가까운 적 찾기
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        List<GameObject> enemys = GameManager.Instance.GetEnemyListInStage();

        GameObject neareastObject = enemys // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
            .OrderBy(obj =>
        {
            return Vector3.Distance(transform.position, obj.transform.position);
        })
        .FirstOrDefault(x => (transform.position - x.transform.position).sqrMagnitude < attackRange * attackRange);

        return neareastObject;
    }

    private void PlayerAttackAnimation()
    {
        playerAnimationController.SetTrigger("Attack");
        transform.LookAt(targetTrm);
    }

    public void ThrowShuriken()
    {
        if (playerInput.IsMoving)
        {
            return;
        }

        Shuriken shuriken = PoolManager.GetItem<Shuriken>("Shuriken1");

        shuriken.ShurikenMoveInit(throwPos, targetTrm.position - transform.position, shurikenSpeed);
        shuriken.ShurikenAttackInit(attackDamage);
    }

    public void AssassinateEnemy()
    {
        //뒤로이동
        Vector3 targetPos;
        targetPos = nowTarget.transform.position;
        targetPos -= nowTarget.transform.forward * 2.5f;
        targetPos.y = transform.position.y;
        transform.position = targetPos;

        //뒤를 보게함
        transform.LookAt(nowTarget.transform.position);

        playerAnimationController.SetTrigger("Assassinate");

        nowTarget.GetComponent<EnemyBase>().AssassinateEnemy();

        playerInput.LockInput(4f);
        CameraManager.Instance.UseActionCam(3f);
        

    }
}
