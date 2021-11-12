using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyLeft : MonoBehaviour
{
    public Text leftText;
    private int leftEnemy;

    public void Update()
    {
        leftEnemy = GameManager.Instance.GetEnemyListInStage().Count;
        if(leftEnemy == 0)
        {
            leftText.text = $"Ŭ����";
        }
       else
        {
            leftText.text = $"���� �� : {leftEnemy}����";
        }
    }
}
