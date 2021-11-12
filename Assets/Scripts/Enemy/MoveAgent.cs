using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    public Transform wayPointGroup;
    private List<Transform> wayPoints = new List<Transform>();
    //��������Ʈ�� ������ ����Ʈ

    public int nextIndex;
    private NavMeshAgent agent;

    public float patrolSpeed = 1.5f;
    public float traceSpeed = 4.0f;

    private bool _patrolling;
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                MoveWayPoint();
            }
        }
    }

    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get
        {
            return agent.velocity.magnitude;
        }
    }

    private void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        agent.enabled = false;
        agent.enabled = true;

        agent.speed = patrolSpeed; //ó�� ��Ʈ�� �ӵ���
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    void Start()
    {
        _patrolling = true;
        wayPointGroup = GameObject.Find("WaypointGroup").transform;
        wayPointGroup.GetComponentsInChildren<Transform>(wayPoints);
        wayPoints.RemoveAt(0);
        MoveWayPoint(); //���� ��������Ʈ�� �̵��Ѵ�.
    }

    private void MoveWayPoint()
    {
        //��ΰ� ���� �����Ǿ� ���� �ʴٸ� true�� �����Ѵ�.
        if (agent.isPathStale) return;

        agent.destination = wayPoints[nextIndex].position;

        agent.isStopped = false; //������Ʈ�� on���ش�.
    }

    // Update is called once per frame
    void Update()
    {
        if (!_patrolling) return;

        if (agent.velocity.sqrMagnitude >= 0.04f && agent.remainingDistance <= 0.5f)
        {
            nextIndex = (++nextIndex) % wayPoints.Count;
            MoveWayPoint();
        }
    }
}
