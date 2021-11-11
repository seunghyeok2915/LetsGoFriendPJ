using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenAround : MonoBehaviour
{
    public Shuriken[] shurikens;

    private const float rotateSpeed = 200f;
    private float rot;

    public void Update()
    {
        rot += Time.deltaTime * rotateSpeed;
        transform.rotation = Quaternion.Euler( new Vector3(0, rot, 0));
    }

    public void SetData(float damage)
    {
        for (int i = 0; i < shurikens.Length; i++)
        {
            shurikens[i].ShurikenAttackInit(damage);
            shurikens[i].isDestoryable = false;
        }
    }
}
