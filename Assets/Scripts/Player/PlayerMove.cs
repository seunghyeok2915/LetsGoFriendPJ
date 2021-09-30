using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInput playerInput;

    private Vector3 moveDir;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.GetMoveInput() == Vector3.zero) return;

        Rotate();
        characterController.Move(transform.forward * moveSpeed * Time.deltaTime);
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
        return characterController.velocity.sqrMagnitude * characterController.velocity.sqrMagnitude;
    }
}
