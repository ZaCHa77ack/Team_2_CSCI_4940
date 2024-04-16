using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPatrolController : MonoBehaviour
{
    public FloatingHealthBar healthBar;

    private Animator anim;
    private Transform target;

    public Vector2 patrolAreaMin;
    public Vector2 patrolAreaMax;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float maxRange;

    [SerializeField]
    private float minRange;

    [SerializeField]
    private float minIdleTime;

    [SerializeField]
    private float maxIdleTime;

    private float idleTime;
    private float idleTimer = 0f;

    private Vector3 nextPoint;

    Rigidbody2D rb;

    [SerializeField]
    private int maxHealth = 100;
    private int currentHealth;

    [SerializeField]
    private Collider2D attackHitbox;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float attackCooldown;
    private float lastAttackTime;

    [SerializeField] new private Collider2D collider;

    // Invincibility Frames
    public float timeInvulnerable = 2.0f;
    public bool isInvulnerable;
    float damageCooldown;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().transform;
        GenerateNextPatrolPoint();

        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.SetMaxHealth(maxHealth);

        collider = GetComponent<Collider2D>();
    }

    public void Update()
    {
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);

        if (distanceToPlayer <= maxRange && distanceToPlayer >= minRange)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer > maxRange)
        {
            Patrol();
        }
        else if (distanceToPlayer < minRange)
        {
            Attack();
        }
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, nextPoint) < 0.25f)
        {
            if (idleTimer <= 0f)
            {
                idleTimer = Random.Range(minIdleTime, maxIdleTime);
                anim.SetBool("isRunning", false);
            }

            if (idleTimer > 0f)
            {
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0f)
                {
                    GenerateNextPatrolPoint();
                }
            }
        }
        else
        {
            MoveTowards(nextPoint);
        }
    }

    public void ChasePlayer()
    {
        anim.SetBool("isRunning", true);
        anim.SetFloat("Move X", (target.position.x - transform.position.x));
        anim.SetFloat("Move Y", (target.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

        if (IsPathBlocked(direction))
        {
            GenerateNextPatrolPoint();
        }
        else
        {
            anim.SetBool("isRunning", true);
            anim.SetFloat("Move X", direction.x);
            anim.SetFloat("Move Y", direction.y);
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            rb.MovePosition(newPosition);
        }
    }

    void GenerateNextPatrolPoint()
    {
        float randX = Random.Range(patrolAreaMin.x, patrolAreaMax.x);
        float randY = Random.Range(patrolAreaMin.y, patrolAreaMax.y);
        nextPoint = new Vector3(randX, randY, transform.position.z);
    }

    bool IsPathBlocked(Vector3 direction)
    {
        float checkDistance = 1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, checkDistance);
        return hit.collider != null && hit.collider.CompareTag("Wall");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            GenerateNextPatrolPoint();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
        {
            return;
        }

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        isInvulnerable = true;
        damageCooldown = timeInvulnerable;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");
            animator.SetBool("isRunning", false);
        }
    }

    public void EnableAttackHitbox()
    {
        attackHitbox.enabled = true;
    }

    public void DisableAttackHitbox()
    {
        attackHitbox.enabled = false;
    }

    void Die()
    {
        collider.enabled = false;
        Destroy(gameObject);
    }
}