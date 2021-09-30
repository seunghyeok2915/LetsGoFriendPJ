using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    public bool IsMoving => joystick.Direction != Vector2.zero;

    public bool CanProcessInput()
    {
        return true; //Cursor.lockState == CursorLockMode.Locked; // 나중에 게임이 끝났는지도 체크하는것 추가해야함.
    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(joystick.Horizontal, 0f,
                joystick.Vertical);

            // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }
}
