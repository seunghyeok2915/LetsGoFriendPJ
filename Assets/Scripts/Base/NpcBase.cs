using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcBase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DoAction();
        }
    }

    public abstract void DoAction();
}
