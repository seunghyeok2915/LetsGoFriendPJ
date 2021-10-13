using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as GameManager;
        }
        GetEnemyInStage();
        FindPlayer();
    }

    protected virtual void OnDestroy()
    {
        _instance = default;
    }

    private List<GameObject> enemyListInStage = new List<GameObject>();
    private GameObject player;

    private void Start()
    {
        PoolManager.CreatePool<Shuriken>("Shuriken1", this.gameObject, 5);
    }

    private void GetEnemyInStage()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyListInStage.Add(item);
        }
    }

    public GameObject GetPlayer() => player;
    public List<GameObject> GetEnemyListInStage() => enemyListInStage;

    private void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }
}
