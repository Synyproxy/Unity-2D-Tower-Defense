using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatus
{
    Next,
    Play,
    GameOver,
    Win
};

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private int totalWaves = 10;

    [SerializeField]
    private Text totalMoneyLbl;

    [SerializeField]
    private Text playBtnLbl;

    [SerializeField]
    private Button playBtn;

    [SerializeField]
    private Text currentWaveLbl;

    [SerializeField]
    private Text totalEscapedLbl;

    [SerializeField]
    private GameObject spawnPoint;

    [SerializeField]
    private Enemy[] enemies;

    [SerializeField]
    private int totalEnemies = 3;

    [SerializeField]
    private int enemiesPerSpawn;

    [SerializeField]
    const float spawnDelay = 0.5f;

    public List<Enemy> enemyList = new List<Enemy>();

    private int waveNumber = 0;
    private int totalMoney = 10;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int enemySelector = 0;
    private GameStatus currentState = GameStatus.Play;

    private AudioSource audioSource;
    private int enemiesToSpawn = 0;

    private void Start()
    {
        playBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>(); 
        ShowMenu();
    }

    private void Update()
    {
        HandleEscpace();
    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && enemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; ++i)
            {
                if (enemyList.Count < totalEnemies)
                {
                    Enemy newEmey = Instantiate(enemies[Random.Range(0, enemiesToSpawn)]) as Enemy;
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

    public void AddMoney(int amount)
    {
        totalMoney += amount;
        totalMoneyLbl.text = totalMoney.ToString();
    }

    public void SubtractMoney(int amount)
    {
        totalMoney -= amount;
        totalMoneyLbl.text = totalMoney.ToString();
    }

    public void ShowMenu()
    {
        switch (currentState)
        {
            case GameStatus.GameOver:
                playBtnLbl.text = "Play Again";
                audioSource.PlayOneShot(SoundManager.Instance.GameOver);
                break;

            case GameStatus.Next:
                playBtnLbl.text = "Next Wave";
                break;

            case GameStatus.Play:
                playBtnLbl.text = "Play";
                break;

            case GameStatus.Win:
                playBtnLbl.text = "Play";
                break;
        }

        playBtn.gameObject.SetActive(true);
    }

    private void HandleEscpace()
    {
        totalEscapedLbl.text = "Escaped " + TotalEscaped + "/10";
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDragSprite();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }

    public void IsWaveOver()
    {
        totalEscapedLbl.text = "Escaped " + TotalEscaped + "/10";

        //Wave is over
        if(roundEscaped + totalKilled == totalEnemies)
        {
            if (waveNumber <= enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }

            SetCurrentGameState();
            ShowMenu();
        }
    }

    public void PlayBtnPressed()
    {
        switch(currentState)
        {
            case GameStatus.Next:
                ++waveNumber;
                totalEnemies += waveNumber;
                break;
            default:
                totalEnemies = 3;
                totalEscaped = 0;
                totalMoney = 10;
                enemiesToSpawn = 0;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagsBuildSites();
                totalMoneyLbl.text = totalMoney.ToString();
                currentWaveLbl.text = "Wave " + waveNumber.ToString();
                totalEscapedLbl.text = "Escaped " + TotalEscaped + "/10";
                audioSource.PlayOneShot(SoundManager.Instance.NewGame); 

                break;
        }

        DestroyAllEnemies();
        TotalKilled = 0;
        roundEscaped = 0;
        currentWaveLbl.text = "Wave " + (waveNumber + 1);
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
    }

    public void SetCurrentGameState()
    {
        if(totalEscaped >= 10)
        {
            currentState = GameStatus.GameOver;
        } else if (waveNumber == 0 && (totalKilled + roundEscaped) == 0)
        {
            currentState = GameStatus.Play;
        } else if(waveNumber >= totalWaves)
        {
            currentState = GameStatus.Win;
        }
        else
        {
            currentState = GameStatus.Next;
        }
    }

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }

        set
        {
            totalEscaped = value;
        }
    }

    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }

        set
        {
            roundEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }

        set
        {
            totalKilled = value;
        }
    }

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }

        set
        {
            totalMoney = value;
        }
    }

    public AudioSource AudioSource
    {
        get
        {
            return audioSource;  
        }
    }
}