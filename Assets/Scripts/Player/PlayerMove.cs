using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerHealth playerHealth;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (playerHealth.isDead)
        {
            rigid.velocity = Vector3.zero;
            return;//죽었을땐 움직이지못함  
        }

        Move();
    }

    private void Move()
    {
        if (playerInput.GetMoveInput() == Vector3.zero) //인풋이 없을땐 안움직임
        {
            rigid.velocity = Vector3.zero;
            return;
        }
        else
        {
            rigid.velocity = transform.forward * moveSpeed;
            Rotate();
        }

    }

    private void Rotate()
    {
        Vector3 v = playerInput.GetMoveInput();
        float degree = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
        float rot = Mathf.LerpAngle(
                        transform.eulerAngles.y,
                        degree,
                        Time.deltaTime * rotateSpeed);
        transform.eulerAngles = new Vector3(0, rot, 0);
    }

    public float GetMagnitude()
    {
        return rigid.velocity.sqrMagnitude * rigid.velocity.sqrMagnitude;
    }
}
