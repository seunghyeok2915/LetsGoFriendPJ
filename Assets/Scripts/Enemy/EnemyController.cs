using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [System.Serializable]
    public struct RendererIndexData
    {
        public Renderer Renderer;
        public int MaterialIndex;

        public RendererIndexData(Renderer renderer, int index)
        {
            Renderer = renderer;
            MaterialIndex = index;
        }
    }

    [Header("인스펙터 세팅")]
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private SkinnedMeshRenderer[] materials;
    [Header("몬스터 세팅")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDamage;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();

    }

    private void Start()
    {
        materials = GetComponentsInChildren<SkinnedMeshRenderer>();
        health.onDamage.AddListener(DamagedEffect);
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
