using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public FloatingHealthBar healthBar;

    private Animator anim;
    private Transform target;
    public Transform homePosition;
    public List<Transform> patrolPoints;
    private int currentPatrolPoint = 0;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float maxRange;

    [SerializeField]
    private float minRange;

    [SerializeField]
    private float waitTime;

    [SerializeField]
    private float currentWaitTime;

    [SerializeField]
    private int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        anim = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().transform;
        if (patrolPoints.Count > 0)
        {
            currentPatrolPoint = 0;
            currentWaitTime = waitTime;
        }

        currentHealth = maxHealth;

        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);

        if (distanceToPlayer <= maxRange && distanceToPlayer >= minRange)
        {
            FollowPlayer();
        }
        else if (distanceToPlayer > maxRange)
        {
            if (patrolPoints.Count > 0)
            {
                Patrol();
            }
            else
            {
                GoHome();
            }
        }
        if (Vector3.Distance(transform.position, homePosition.position) < 0.1f)
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isAttacking", false);
        }
    }

    public void FollowPlayer()
    {
        anim.SetBool("isRunning", true);
        anim.SetFloat("Move X", (target.position.x - transform.position.x));
        anim.SetFloat("Move Y", (target.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    public void GoHome()
    {
        MoveTowards(homePosition.position);
    }

    public void Patrol()
    {
        Transform patrolPoint = patrolPoints[currentPatrolPoint];
        if (Vector3.Distance(transform.position, patrolPoint.position) < 0.2f)
        {
            if (currentWaitTime <= 0)
            {
                currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Count;
                currentWaitTime = waitTime;
            }
            else
            {
                currentWaitTime -= Time.deltaTime;
                anim.SetBool("isRunning", false);
            }
        }
        else
        {
            anim.SetBool("isRunning", true);
            MoveTowards(patrolPoint.position);
        }
    }
    
    private void MoveTowards(Vector3 targetPosition)
    {
        anim.SetBool("isRunning", true);
        anim.SetFloat("Move X", (homePosition.position.x - transform.position.x));
        anim.SetFloat("Move Y", (homePosition.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

}
