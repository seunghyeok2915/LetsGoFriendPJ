using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Utils : MonoBehaviour
{
    public static GameObject FindNearestEnemy(Transform trans,float range,GameObject exceptEnemy = null) //가까운 적 찾기
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        List<GameObject> enemys = GameManager.Instance.GetEnemyListInStage().ToList();

        if(exceptEnemy != null)
        {
            enemys.Remove(exceptEnemy);
        }

        GameObject neareastObject = enemys // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
            .OrderBy(obj =>
            {
                return Vector3.Distance(trans.position, obj.transform.position);
            })
        .FirstOrDefault(x => (trans.position - x.transform.position).sqrMagnitude < range * range);

        return neareastObject;
    }
}
