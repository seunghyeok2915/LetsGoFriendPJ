using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("인스펙터 세팅")]
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private SkinnedMeshRenderer[] materials;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [Header("몬스터 세팅")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackDamage;

    private GameObject player;
    private float distance;
    private float lastTimeAttack = 0;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        materials = GetComponentsInChildren<SkinnedMeshRenderer>();
        health.onDamage.AddListener(DamagedEffect);
        player = GameManager.Instance.GetPlayer();

        navMeshAgent.speed = moveSpeed;
        navMeshAgent.autoBraking = false;
    }

    private void DamagedEffect()
    {
        foreach (var item in materials)
        {
            item.material.DOColor(Color.red, 0.2f).OnComplete(() => item.material.DOColor(Color.white, 0.2f));
            print("적 색체크");
        }
    }
}
