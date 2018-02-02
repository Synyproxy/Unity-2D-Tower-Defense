using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private int healthPoints;

    [SerializeField]
    private Transform exitPoint;

    [SerializeField]
    private Transform[] wayPoints;

    [SerializeField]
    private float navigationUpdate;

    private Transform enemy;
    private Collider2D enemyCollider;
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
        GameManager.Instance.RegisterEnemy(this);
        enemyCollider = GetComponent<BoxCollider2D>();
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
            //Hurt Animation
        }
        else
        {
            //Enemy die and animation
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        enemyCollider.enabled = false;
    }
}
