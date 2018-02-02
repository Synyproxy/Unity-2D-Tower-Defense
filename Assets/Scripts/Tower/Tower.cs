using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour {

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private Projectile projectile;
    private Enemy targetEnemy = null;
    private float attackCounter;
    private bool canAttack = false;

    void Start ()
    {
    
	}
	
	void Update ()
    {
        attackCounter -= Time.deltaTime;

        //No Enemy Targeted 
        if(targetEnemy ==  null || targetEnemy.IsDead)
        {
            //Search for target
            Enemy nearestEnemy = GetNearestEnemyInRange();
             
            //Target Acquired
            if(nearestEnemy != null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRange)
            {
                targetEnemy = nearestEnemy;
            }
        }else
        {
            if(attackCounter <= 0)
            {
                canAttack = true;

                attackCounter = attackCooldown;
            }else
            {
                canAttack = false;
            }

            //Enemy out of Range 
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRange)
            {
                targetEnemy = null;
            }
        }


    }

    private void FixedUpdate()
    {
        if (canAttack)
            Attack();

    }

    public void Attack()
    {
        canAttack = false;
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;

        if(targetEnemy == null)
        {
            Destroy(newProjectile);
        }
        else
        {
            StartCoroutine(MoveProjecile(newProjectile));
        }
    }

    IEnumerator MoveProjecile(Projectile projectile)
    {
        //enemy in range and projectile is ready
        while(getTargetDistance(targetEnemy) > 0.2f && projectile != null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            float angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);

            yield return null;
        }

        //projectile destroyed or target dead or outofrange
        if(projectile != null || targetEnemy == null)
        {
            Destroy(projectile);
        }
    }

    private float getTargetDistance(Enemy enemy)
    {
        if(enemy == null)
        {
            enemy = GetNearestEnemyInRange();

            if (enemy == null)
                return 0f;
        }

        return Mathf.Abs(Vector2.Distance(transform.localPosition, enemy.transform.localPosition));
    }

    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach(Enemy enemy in GameManager.Instance.enemyList)
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRange)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange;
    }

    private Enemy GetNearestEnemyInRange()
    {
        Enemy nearestEnemy = null;

        float smallestDistance = float.PositiveInfinity;

        float distanceTowerEnemy;

        foreach (Enemy enemy in GetEnemiesInRange())
        {
            distanceTowerEnemy = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
            if (distanceTowerEnemy < smallestDistance)
            {
                smallestDistance = distanceTowerEnemy;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
