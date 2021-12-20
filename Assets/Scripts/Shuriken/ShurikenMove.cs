using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenMove : MonoBehaviour
{
    private float moveSpeed;
    private float rotateSpeed = 500f;

    private float boomerangDist = 50f;
    private bool boomerangMode = false;

    private Vector3 startPos;
    private Vector3 moveDir;

    public void ShurikenMoveInit(Transform startPosition, Vector3 moveDir, float moveSpeed)
    {
        boomerangMode = false;

        startPos = startPosition.position;
        transform.position = startPosition.position;

        moveDir.y = 0;
        this.moveDir = moveDir.normalized;

        this.moveSpeed = moveSpeed;
    }

    public void ChangeDir(Vector3 moveDir)
    {
        moveDir.y = 0;
        this.moveDir = moveDir.normalized;
    }

    public void Move()
    {
        if (boomerangMode)
        {
            ChangeDir(GameManager.Instance.GetPlayer().transform.position - transform.position);

            if (Vector3.Distance(GameManager.Instance.GetPlayer().transform.position,transform.position) < 2f)
            {
                print("»ç¶óÁü");
                gameObject.SetActive(false);
            }
        }

        transform.Translate(moveDir * Time.deltaTime * moveSpeed, Space.World);
        transform.Rotate(new Vector3(0, rotateSpeed, 0));

        if (GameManager.Instance.GetPlayer().GetComponent<PlayerStats>().CanUseSkill(ESkill.Boomerang) && !boomerangMode)
        {
            if(Vector3.Distance(startPos,transform.position) > boomerangDist)
            {
                boomerangMode = true;
            }
        }
    }
}
