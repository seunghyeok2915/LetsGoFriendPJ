using UnityEngine;

public class Enemy_Split : EnemyBase
{
    public int nowGrade;
    public float moveSpeed;
    private Vector3 moveDir;
    private Rigidbody rb;

    public override void Start()
    {
        base.Start();
        SetSplitEnemy();
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (isDead)
        {
            return;
        }
        rb.velocity = moveDir * moveSpeed;
        //transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private Vector3 GetRandomDir() // 랜덤 방향 가져오기
    {
        float degree = Random.Range(0f, 360f);

        Vector3 randDir = new Vector3(Mathf.Sin(degree * Mathf.Rad2Deg), 0, Mathf.Cos(degree * Mathf.Rad2Deg) ).normalized;
        return randDir;
    }

    public void SetSplitEnemy()
    {
        moveDir = GetRandomDir();
        switch (nowGrade)
        {
            case 1:
                MaxHealth = 25;
                break;
            case 2:
                MaxHealth = 50;
                break;
            case 3:
                MaxHealth = 100;
                break;
            default:
                break;
        }

        SetHP();
        SetHpBar();
    }

    public override void StartEnemy() //적행동 시작
    {
        base.StartEnemy();
        //StartCoroutine(ShootCoroutine());
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.CompareTag("Wall"))
        {
            
            //튕기기
            ContactPoint cp = other.GetContact(0);

            //Vector3 inNormal = Vector3.Normalize(transform.position - other.collider.transform.position);
            Vector3 bounceVector = Vector3.Reflect(moveDir, cp.normal);
            bounceVector = bounceVector.normalized;

            moveDir = bounceVector;

        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (isDead)
            {
                return; // 죽으면 공격안함
            }

            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.OnDamage(GetTotalDamage());
            }
        }
    }

    private float GetTotalDamage()
    {
        totalDamage = normalDamage; // 데미지 계산
        return totalDamage;
    }

    protected override void Die()
    {
        base.Die();

        //죽을때 자식들생성해줘야함
    }
}
