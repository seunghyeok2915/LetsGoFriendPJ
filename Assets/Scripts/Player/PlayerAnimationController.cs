using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        animator.SetBool("isMoving", playerInput.IsMoving);

    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void SetAttackAnimSpeed(float speed)
    {
        animator.SetFloat("AttackSpeed", speed);
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Die");
    }

    public void OnRevivePlayer()
    {
        animator.SetTrigger("Revive");
    }
}
