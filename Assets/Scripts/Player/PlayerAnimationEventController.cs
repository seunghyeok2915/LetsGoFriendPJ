using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventController : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
    }

    public void AttackAnimationEvent()
    {
        playerAttack.ThrowShuriken();
    }
}
