using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as CameraManager;
        }
    }

    protected virtual void OnDestroy()
    {
        _instance = default;
    }

    public Camera mainCam;
    public Camera actionCam;

    private Vector3 cameraPos;

    private float shakeRange;
    private float duration;

    public void Shake(float shakeRange, float duration)
    {
        this.shakeRange = shakeRange;
        this.duration = duration;

        cameraPos = mainCam.transform.position;
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }

    void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos = mainCam.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        mainCam.transform.position = cameraPos;
    }

    void StopShake()
    {
        CancelInvoke("StartShake");
    }

    public void UseActionCam(float time)
    {
        StartCoroutine(OnUseActionCam(time));
    }

    private IEnumerator OnUseActionCam(float time)
    {
        mainCam.enabled = false;
        actionCam.enabled = true;
        yield return new WaitForSeconds(time);
        mainCam.enabled = true;
        actionCam.enabled = false;
    }
}
