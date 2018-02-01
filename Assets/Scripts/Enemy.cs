using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private int target = 0;

    [SerializeField]
    private Transform exitPoint;

    [SerializeField]
    private Transform[] wayPoints;

    [SerializeField]
    private float navigationUpdate;

    private Transform enemy;
    private float navigationTime = 0;

    void Start ()
    {
        enemy = GetComponent<Transform>();
	}
	
	void Update ()
    {
        if(wayPoints != null)
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
        //Enemy hit 
        if (other.tag == "WayPoint")
        {
            ++target;
        }
        else if (other.tag == "Finish")
        {
            GameManager.instance.RemoveEnemyFromScreen();
            Destroy(gameObject);
        }
            
    }
}
