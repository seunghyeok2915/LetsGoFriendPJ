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
