using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenMove : MonoBehaviour
{
    private float moveSpeed;
    private float rotateSpeed = 500f;

    private Vector3 moveDir;

    public void ShurikenMoveInit(Transform startPosition, Vector3 moveDir, float moveSpeed)
    {
        transform.position = startPosition.position;

        moveDir.y = 0;
        this.moveDir = moveDir.normalized;

        this.moveSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        transform.Translate(moveDir * Time.deltaTime * moveSpeed, Space.World);
        transform.Rotate(new Vector3(0, rotateSpeed, 0));
    }

}
