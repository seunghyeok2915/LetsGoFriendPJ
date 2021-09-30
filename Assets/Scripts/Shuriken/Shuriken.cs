using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour, IPoolable
{
    private ShurikenAttack shurikenAttack;
    private ShurikenMove shurikenMove;

    private void Awake()
    {
        shurikenMove = GetComponent<ShurikenMove>();
        shurikenAttack = GetComponent<ShurikenAttack>();
    }

    public void ShurikenMoveInit(Transform startPosition, Vector3 moveDir, float moveSpeed)
    {
        shurikenMove.ShurikenMoveInit(startPosition, moveDir, moveSpeed);
    }

    public void ShurikenAttackInit(float damage)
    {
        shurikenAttack.ShurikenAttackInit(damage);
    }

    public void OnPool()
    {

    }
}
