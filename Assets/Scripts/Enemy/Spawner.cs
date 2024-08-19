using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] int alive = 50, maxEnemyAtOnce = 6;
    [SerializeField] float areaRange = 45f;
    [SerializeField] float timeSpawn = 0.5f;
    private float timer = 0f;
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
        DisplayAlive();
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
        EnemyName enemyName = (EnemyName)Random.Range(-1, 3) + 1;
        enemy.WhenOnGet(randomPosition, enemyName.ToString(), randomSkin, randomSkin);
        enemyCount++;
        enemy.gameObject.SetActive(true);
    }

    private void OnRelease(EnemyController enemy)
    {
        enemyCount--;
        enemy.gameObject.SetActive(false);
        enemy.ResetOnRelease();
    }

    private void DisplayAlive()
    {
        if (GameObject.Find("Canvas").GetComponent<UIController>() != null)
        {
            GameObject.Find("Canvas").GetComponent<UIController>().DisplayAliveCount(alive);
        }
    }

    public void ReduceAlive()
    {
        alive--;
        DisplayAlive();
    }
    public int GetAlive()
    {
        return alive;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyCount < maxEnemyAtOnce && alive - enemyCount - 1 > 0)
        {
            timer += Time.deltaTime;
            if(timer >= timeSpawn)
            {
                enemyPool.Get();
                timer = 0;
            }
        }
    }
}
