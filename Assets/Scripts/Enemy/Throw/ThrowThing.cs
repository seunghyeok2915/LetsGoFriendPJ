using UnityEngine;

public class ThrowThing : MonoBehaviour
{
    private const float liveTime = 5f; //5���ĸ� ������ �������
    private float speed;
    private float damage;

    private Vector3 moveDir = Vector3.zero;

    public void SetData(float speed, float damage, Vector3 targetPos, bool isDir = false)
    {
        this.speed = speed;
        this.damage = damage;
        if(!isDir)
        {
            moveDir = (targetPos - transform.position).normalized;
            moveDir.y = 0;
        }
        else
        {
            moveDir = targetPos.normalized;
            moveDir.y = 0;
        }

        Invoke(nameof(SetFalse),liveTime);
    }

    private void SetFalse()
    {
        gameObject.SetActive(false);
    }

    public void FixedUpdate()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.OnDamage(damage);
            }
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            CancelInvoke(nameof(SetFalse));
            gameObject.SetActive(false);
        }
    }
}
