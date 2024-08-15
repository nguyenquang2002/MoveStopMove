using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] int alive = 50, maxEnemyAtOnce = 6;
    [SerializeField] float areaRange = 45f;

    [SerializeField] Material[] listSkin;
    private int enemyCount = 0;

    [SerializeField] Attack playerAttack;

    [SerializeField] EnemyController enemyPrefabs;
    private IObjectPool<EnemyController> enemyPool;

    public enum EnemyName
    {
        Hunter,
        Ranger,
        Knight,
        Lancer,
        Assassin
    };

    private void Awake()
    {
        enemyCount = 0;
        enemyPool = new ObjectPool<EnemyController>(CreateEnemy, OnGet, OnRelease);
    }

    private EnemyController CreateEnemy()
    {
        EnemyController enemy = Instantiate(enemyPrefabs);
        enemy.SetPool(enemyPool);
        return enemy;
    }

    private void OnGet(EnemyController enemy)
    {
        Vector3 randomPosition;
        do
        {
            randomPosition = new Vector3(Random.Range(-areaRange, areaRange) / 2, 0.15f, Random.Range(-areaRange, areaRange) / 2);
        } while (Vector3.Distance(randomPosition, playerAttack.transform.position) < playerAttack.Range() + 1);
        Material randomSkin = listSkin[Random.Range(0, listSkin.Length)];
        EnemyName enemyName = (EnemyName)Random.Range(-1, 4) + 1;
        enemy.WhenOnGet(randomPosition, enemyName.ToString(), randomSkin, randomSkin);
        enemyCount++;
        enemy.gameObject.SetActive(true);
    }

    private void OnRelease(EnemyController enemy)
    {
        enemyCount--;
        alive--;
        enemy.gameObject.SetActive(false);
        enemy.ResetOnRelease();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyCount < maxEnemyAtOnce && alive - 1 > 0)
        {
            enemyPool.Get();
        }
    }
}
