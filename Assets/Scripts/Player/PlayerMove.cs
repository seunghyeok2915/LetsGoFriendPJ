using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerHealth playerHealth;

    private Vector3 moveDir;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (playerHealth.isDead)
        {
            rigidbody.velocity = Vector3.zero;
            return;//죽었을땐 움직이지못함  
        }

        Rotate();
        Move();
    }

    private void Move()
    {
        if (playerInput.GetMoveInput() == Vector3.zero) //인풋이 없을땐 안움직임
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }

        rigidbody.velocity = transform.forward * moveSpeed;
    }

    private void Rotate()
    {
        Vector3 v = playerInput.GetMoveInput();
        float degree = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
        float rot = Mathf.LerpAngle(
                        transform.eulerAngles.y,
                        degree,
                        Time.deltaTime * rotateSpeed);
        transform.eulerAngles = new Vector3(0, rot, 0);
    }

    public float GetMagnitude()
    {
        return rigidbody.velocity.sqrMagnitude * rigidbody.velocity.sqrMagnitude;
    }
}
