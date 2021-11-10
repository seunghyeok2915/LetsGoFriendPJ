using UnityEngine;

public class Effect : MonoBehaviour, IPoolable
{
    public void OnPool()
    {
        Invoke("Disable", 3f);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
