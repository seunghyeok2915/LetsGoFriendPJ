using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform startPos;
    public Potal potal;

    public void Play()
    {
        gameObject.SetActive(true);
        GameManager.Instance.GetPlayer().transform.position = startPos.position;
    }

    public void Stop()
    {
        print("????");
        gameObject.SetActive(false);

    }
}
