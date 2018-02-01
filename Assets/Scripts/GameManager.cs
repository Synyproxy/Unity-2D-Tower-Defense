using System.Collections;
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

    private int enemiesOnScreen = 0;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    public void RemoveEnemyFromScreen()
    {
        if (enemiesOnScreen > 0)
            --enemiesOnScreen;
    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && enemiesOnScreen < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; ++i)
            {
                if (enemiesOnScreen < maxEnemiesOnScreen)
                {
                    GameObject newEmey = Instantiate(enemies[0]) as GameObject;
                    newEmey.transform.position = spawnPoint.transform.position;

                    ++enemiesOnScreen;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

}