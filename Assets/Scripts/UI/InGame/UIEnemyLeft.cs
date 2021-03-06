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
            leftText.text = $"다음 스테이지로 이동";
        }
       else
        {
            leftText.text = $"남은 적 : {leftEnemy}마리";
        }
    }
}
