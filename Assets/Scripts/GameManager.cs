using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

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

    private int enemiesOnScreen;

    private void Awake()    //Singleton Pattern with Loader script on the Camera
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if(enemiesPerSpawn > 0 && enemiesOnScreen < totalEnemies)
        {
            for(int i = 0; i < enemiesPerSpawn; ++i)
            {
                if(enemiesOnScreen < maxEnemiesOnScreen)
                {
                    GameObject newEmey = Instantiate(enemies[0]) as GameObject;
                    newEmey.transform.position = spawnPoint.transform.position;

                    ++enemiesOnScreen;
                }
            }
        }
    }

    public void RemoveEnemyFromScreen()
    {
        if (enemiesOnScreen > 0)
            --enemiesOnScreen;
    }

}