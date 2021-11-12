using UnityEngine;
using UnityEngine.Events;

public class Potal : MonoBehaviour
{
    public UnityAction onEnter;

    public void SetEvent(UnityAction callback)
    {
        onEnter = callback;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            onEnter?.Invoke();
            
        }
    }
}
