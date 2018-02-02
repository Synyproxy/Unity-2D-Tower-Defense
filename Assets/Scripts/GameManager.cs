using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject spawnPoint;

    [SerializeField]
    private GameObject[] enemies;

    [SerializeField]
    private int maxEnemiesOnScreen;

    [SerializeField]
    private int totalEnemies;

    [SerializeField]
    private int enemiesPerSpawn;

    [SerializeField]
    const float spawnDelay = 0.5f;

    public List<Enemy> enemyList = new List<Enemy>();

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && enemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; ++i)
            {
                if (enemyList.Count < maxEnemiesOnScreen)
                {
                    GameObject newEmey = Instantiate(enemies[0]) as GameObject;
                    newEmey.transform.position = spawnPoint.transform.position;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyAllEnemies()
    {
        foreach(Enemy enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }

        enemyList.Clear();
    }
}