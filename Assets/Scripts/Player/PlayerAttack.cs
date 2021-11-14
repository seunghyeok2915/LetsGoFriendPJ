using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("인스펙터 세팅")]
    [SerializeField] private PlayerInput playerInput; //플레이어 인풋 관리
    [SerializeField] private PlayerAnimationController playerAnimationController; //플레이어 애니메이션 관리
    [SerializeField] private PlayerHealth playerHealth; //플레이어 체력관리
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Transform throwPos; // 표창 나갈곳
    [Header("플레이어 세팅")]
    public float attackDamage; //공격 데미지
    public float attackDelay; // 몇초마다 공격할건지
    public float attackRange; // 공격 거리
    public float assassinateRange; //거리
    public float shurikenSpeed; // 표창 속도

    private GameObject nowTarget;
    private Transform targetTrm;
    private float tempAttackTimer;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerHealth = GetComponent<PlayerHealth>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        //SetAnimationSpeed();
    }

    public  void AttackDelayUp(float delay)
    {
        attackDelay -= delay;
    }
    public bool CanAssassinate()
    {
        nowTarget = Utils.FindNearestEnemy(transform, assassinateRange);
        if (nowTarget != null)
        {
            //if (Vector3.Distance(transform.position, nowTarget.transform.position) < assassinateRange)
            //{
            return true;
            //}
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
            nowTarget = Utils.FindNearestEnemy(transform, attackRange);

            if (nowTarget != null)
            {
                targetTrm = nowTarget.transform;
                if (tempAttackTimer > attackDelay)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void PlayerAttackAnimation() //공격 애니메이션 실행함 
    {
        playerAnimationController.SetTrigger("Attack");
        Vector3 lookVec = targetTrm.position;
        lookVec.y = 0;
        transform.LookAt(lookVec);
    }

    public void OnAttack()
    {
        if (playerInput.IsMoving)
        {
            return;
        }
        Vector3 throwDir = targetTrm.position - transform.position;
        bool skillUsed = false;

        if (playerStats.CanUseSkill(ESkill.SideShot))
        {
            print("사이드샷");
            SideShot(throwDir);
            skillUsed = true;
        }

        if (playerStats.CanUseSkill(ESkill.SplitShot))
        {
            print("스플릿샷");
            SplitShot(throwDir);
        }

        if (skillUsed)
        {
            return;
        }

        ThrowShuriken(throwDir);
    }

    private void ThrowShuriken(Vector3 throwDir, bool skillUsed = false) // 공격 표창던지기
    {
        if (playerStats.CanUseSkill(ESkill.TwinShot) && !skillUsed)
        {
            StartCoroutine(TwinShot(throwDir));
            return;
        }

        Shuriken shuriken = PoolManager.GetItem<Shuriken>("Shuriken1");

        shuriken.ShurikenMoveInit(throwPos, throwDir, shurikenSpeed);
        shuriken.ShurikenAttackInit(attackDamage);
    }

    public void SplitShot(Vector3 throwDir)
    {
        ThrowShuriken(RotateVector(throwDir, 45));
        ThrowShuriken(RotateVector(throwDir, -45));
    }

    public IEnumerator TwinShot(Vector3 throwDir)
    {
        ThrowShuriken(throwDir, true);
        yield return new WaitForSeconds(0.1f);
        ThrowShuriken(throwDir, true);
    }

    public void SideShot(Vector3 throwDir)
    {
        ThrowShuriken(throwDir);
        ThrowShuriken(RotateVector(throwDir, 90));
        ThrowShuriken(RotateVector(throwDir, -90));
    }

    public Vector3 RotateVector(Vector3 vector, float angle) //벡터 회전 함수 y 축기준으로 angle 도 회전
    {
        Quaternion qRotate = Quaternion.AngleAxis(angle, Vector3.up); // 해당 축 기준으로 30도 회전한 쿼터니언 값
        return qRotate * vector;
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
