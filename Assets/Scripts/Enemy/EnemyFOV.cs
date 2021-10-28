using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public CircularSectorMeshRenderer circularSectorMeshRenderer;
    public float viewRange = 15.0f;
    [Range(0, 360)]
    public float viewAngle = 120.0f;

    public LayerMask layerMask;
    private Transform playerTr;
    private int playerLayer; //�÷��̾ �����ִ� ���̾�

    //������ 1�� ���� ���ֿ� �ִ� ���� ��ǥ�� ���ϴ� �Լ�
    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),
                            0,
                            Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    void Start()
    {
        playerTr = GameManager.Instance.GetPlayer().transform;
        playerLayer = LayerMask.NameToLayer("Player");
        circularSectorMeshRenderer = GetComponentInChildren<CircularSectorMeshRenderer>();
        //�÷��̾� ���̾��� ��ȣ�� �˾ƿ´�.
    }

    private void Update()
    {
        circularSectorMeshRenderer.degree = viewAngle;
        circularSectorMeshRenderer.radius = viewRange;
    }

    //�þ߹����ȿ� �÷��̾ �ִ°�?
    public bool IsTracePlayer()
    {
        bool isTrace = false;
        Collider[] colls = Physics.OverlapSphere(
            transform.position, viewRange, 1 << playerLayer);
        if (colls.Length == 1)
        {
            Vector3 dir = (playerTr.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dir) < viewAngle * 0.5f)
            {
                isTrace = true;
            }
        }
        return isTrace;
    }

    //�÷��̾�� �� ���̿� �ƹ��͵� ���� �÷��̾ ���̴°�?
    public bool IsViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;
        Vector3 dir = (playerTr.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), dir, out hit, layerMask))
        {
            isView = (hit.collider.gameObject.CompareTag("Player"));
        }
        return isView;
    }
}
