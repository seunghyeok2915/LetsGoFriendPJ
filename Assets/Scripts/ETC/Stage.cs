using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject[] objects;

    public Transform startPos;
    public Potal potal;

    public void Play()
    {
        gameObject.SetActive(true);
        GameManager.Instance.GetPlayer().transform.position = startPos.position;
    }

    public void Stop()
    {
        gameObject.SetActive(true);
    }
}
