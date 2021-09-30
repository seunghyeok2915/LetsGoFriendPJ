using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerTrm;
    public Vector3 camOffSet;

    private void Start()
    {
        playerTrm = GameManager.Instance.player.transform;
    }

    private void Update()
    {
        Vector3 camPos = playerTrm.position;
        camPos.x = 0;
        camPos += camOffSet;

        transform.position = camPos;
    }
}
