using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Utils : MonoBehaviour
{
    public static GameObject FindNearestEnemy(Transform trans,float range,GameObject exceptEnemy = null) //����� �� ã��
    {
        // Ž���� ������Ʈ ����� List �� �����մϴ�.
        List<GameObject> enemys = GameManager.Instance.GetEnemyListInStage().ToList();

        if(exceptEnemy != null)
        {
            enemys.Remove(exceptEnemy);
        }

        GameObject neareastObject = enemys // LINQ �޼ҵ带 �̿��� ���� ����� ���� ã���ϴ�.
            .OrderBy(obj =>
            {
                return Vector3.Distance(trans.position, obj.transform.position);
            })
        .FirstOrDefault(x => (trans.position - x.transform.position).sqrMagnitude < range * range);

        return neareastObject;
    }
}
