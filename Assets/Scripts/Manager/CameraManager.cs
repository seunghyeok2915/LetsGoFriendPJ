using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    protected virtual void OnDestroy()
    {
        Instance = default;
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

    private void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        var cameraPos = mainCam.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        mainCam.transform.position = cameraPos;
    }

    private void StopShake()
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