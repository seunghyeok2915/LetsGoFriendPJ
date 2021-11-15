using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_02 : EnemyBase
{
    public float throwSpeed;
    public float throwDamage;

    public int spawnNum; //몇게 소환하는지
    public float attackDelay; //쏘기전에 기다리는시간
    public float maxDist;

    private Transform playerTrm;
    private LineRenderer lineRenderer;

    public override void Start()
    {
        base.Start();

        playerTrm = GameManager.Instance.GetPlayer().transform;
        StartCoroutine(BossRoutine());

        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.startColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.endColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
    }

    private IEnumerator BossRoutine()
    {
        while (true)
        {
            float restTime = 2f;
            yield return new WaitForSeconds(restTime);
            int randPattern = Random.Range(0, 2);
            switch (randPattern)
            {
                case 0:
                    yield return AttackShoot(); //쏘기
                    break;
                case 1:
                    yield return SpawnPattern();
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator SpawnPattern() //적소환함
    {
        animator.SetTrigger("Attack");
        for (int i = 0; i < spawnNum; i++)
        {
            float range = 2f;
            var radian = i * (360 / (float)6) * Mathf.Deg2Rad;
            var x = (range) * Mathf.Sin(radian);
            var z = (range) * Mathf.Cos(radian);

            Vector3 createPos = new Vector3(x, 0, z) + transform.position;

            var bomb = PoolManager.GetItem<Enemy_Bomb>("Enemy_Bomb");
            bomb.transform.position = createPos;

            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    private IEnumerator AttackShoot()
    {
        float time = Time.time;
        //라인 렌더러 그려줘야해
        while (true)
        {
            transform.LookAt(GameManager.Instance.GetPlayer().transform);
            DrawDangerLine();
            if (Time.time - time > attackDelay)
            {
                break;
            }

            yield return null;
        }

        animator.SetTrigger("Attack");
        ThrowThing throwThing = PoolManager.GetItem<ThrowThing>("Ob_Enemy_Throw");
        throwThing.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        throwThing.SetData(throwSpeed, throwDamage, GameManager.Instance.GetPlayer().transform.position);

        //발사


        lineRenderer.enabled = false;
        yield return null;
    }

    private void DrawDangerLine()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * maxDist);
    }
}
