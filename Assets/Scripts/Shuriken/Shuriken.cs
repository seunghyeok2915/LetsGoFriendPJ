using UnityEngine;

public class Shuriken : MonoBehaviour, IPoolable
{
    public bool isDestoryable = true;
    private ShurikenAttack shurikenAttack;
    private ShurikenMove shurikenMove;
    private const float liveTime = 5f; //5���ĸ� ������ �������



    private void Awake()
    {
        shurikenMove = GetComponent<ShurikenMove>();
        shurikenAttack = GetComponent<ShurikenAttack>();
    }

    private void FixedUpdate()
    {
        if (shurikenMove != null)
        {
            shurikenMove.Move();
        }
    }

    public void ShurikenMoveInit(Transform startPosition, Vector3 moveDir, float moveSpeed)
    {
        shurikenMove.ShurikenMoveInit(startPosition, moveDir, moveSpeed);
    }

    public void ShurikenAttackInit(float damage)
    {
        shurikenAttack.ShurikenAttackInit(damage);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Wall"))
        {
            if (!isDestoryable) return;

            CancelInvoke(nameof(SetFalse));
            gameObject.SetActive(false);
        }

        shurikenAttack.OnAttack(other);

        if (GameManager.Instance.GetPlayer().GetComponent<PlayerStats>().CanUseSkill(ESkill.Boomerang))
        {
            return; //�Ȳ�
        }

        if (GameManager.Instance.GetPlayer().GetComponent<PlayerStats>().CanUseSkill(ESkill.PierceShot))
        {
            return; //���� ó������
        }

        if (other.CompareTag("Enemy"))
        {
            if (!isDestoryable) return;

            CancelInvoke(nameof(SetFalse));
            gameObject.SetActive(false);
        }

    }

    public void OnPool()
    {
        Invoke(nameof(SetFalse), liveTime);
    }

    private void SetFalse()
    {
        gameObject.SetActive(false);
    }
}
