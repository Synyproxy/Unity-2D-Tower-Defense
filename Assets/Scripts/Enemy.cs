using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private int healthPoints;

    [SerializeField]
    private int rewardAmount;

    [SerializeField]
    private Transform exitPoint;

    [SerializeField]
    private Transform[] wayPoints;

    [SerializeField]
    private float navigationUpdate;

    private Transform enemy;
    private Collider2D enemyCollider;
    private Animator anim;
    private int target = 0;
    private float navigationTime = 0;
    private bool isDead = false;
    
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    void Start ()
    {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        GameManager.Instance.RegisterEnemy(this);
	}
	
	void Update ()
    {
        if(wayPoints != null && !isDead)
        {
            navigationTime += Time.deltaTime;

            if(navigationTime > navigationUpdate)
            {
                if(target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
                }

                navigationTime = 0;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "WayPoint")
        {
            ++target;
        }
        else if (other.tag == "Finish")
        {
            GameManager.Instance.UnregisterEnemy(this);
            GameManager.Instance.RoundEscaped += 1;
            GameManager.Instance.TotalEscaped += 1;
            GameManager.Instance.IsWaveOver();

        } else if(other.tag == "Projectile")
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            EnemyHit(projectile.AttackStrength);
            Destroy(other.gameObject);
        }
            
    }

    public void EnemyHit(int hitPoints)
    {
        if(healthPoints - hitPoints > 0)
        {
            healthPoints -= hitPoints;
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
            anim.Play("Hurt");
        }
        else
        {
            Die();
            anim.SetTrigger("didDie");
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
        }
    }

    public void Die()
    {
        isDead = true;
        enemyCollider.enabled = false;
        GameManager.Instance.TotalKilled += 1;
        GameManager.Instance.AddMoney(rewardAmount);
        GameManager.Instance.IsWaveOver();
    }
}
