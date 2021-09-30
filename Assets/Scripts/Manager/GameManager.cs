using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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

        player = GameObject.FindWithTag("Player");
        GetEnemyInStage();
    }

    protected virtual void OnDestroy()
    {
        _instance = default;
    }

    public GameObject player;
    public List<GameObject> enemyListInStage = new List<GameObject>();

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
}
