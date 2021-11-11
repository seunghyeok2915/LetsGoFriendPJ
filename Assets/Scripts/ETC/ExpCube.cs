using System.Collections;
using UnityEngine;

public class ExpCube : MonoBehaviour
{
    public float range = 3;

    public Rigidbody rb;
    private PlayerStats playerStats;
    private float exp;
    private bool canEat = false;

    public void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        playerStats = GameManager.Instance.GetPlayer();
    }
    private void Update()
    {
        //if (Vector3.Distance(transform.position, playerStats.transform.position) < range && canEat)
        //{
        //    GiveExp();
        //}
    }

    public void SetData(float exp, Transform transform)
    {
        this.exp = exp;
        this.transform.position = transform.position;

        float randX = Random.Range(-1f, 1f);
        float randZ = Random.Range(-1f, 1f);
        Vector3 fireDir = new Vector3(randX, 1, randZ).normalized;
        print(fireDir);
        //Invoke("canEatTrue", 1f);
        rb.AddForce(fireDir * 300);
        StartCoroutine(ExpRoutine());
    }

    //private void canEatTrue()
    //{
    //    canEat = true;
    //}
    private IEnumerator ExpRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (Vector3.Distance(transform.position, playerStats.transform.position) > 3f)
        {
            Vector3 dir = (playerStats.transform.position - transform.position).normalized;
            transform.position += dir * 40f * Time.deltaTime;
            yield return null;
        }
        GiveExp();
    }

    private void GiveExp()
    {
        if (GameManager.Instance.IsPlaying)
        {
            playerStats.AddExp(exp);
        }
        exp = 0;
        canEat = false;
        gameObject.SetActive(false);
    }
}
