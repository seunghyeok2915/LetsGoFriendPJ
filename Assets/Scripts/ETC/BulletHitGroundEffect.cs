using UnityEngine;

public class BulletHitGroundEffect : MonoBehaviour ,IPoolable
{
    public void OnPool()
    {
        Invoke("SetActiveFalse", 2f);
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
