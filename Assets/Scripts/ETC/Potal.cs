using UnityEngine;
using UnityEngine.Events;

public class Potal : MonoBehaviour
{
    public UnityAction onEnter;

    public void SetEvent(UnityAction callback)
    {
        onEnter = callback;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onEnter?.Invoke();
        }
    }
}
