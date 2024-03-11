using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPatrolController : MonoBehaviour
{
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

    private Vector3 nextPoint;

    Rigidbody2D rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().transform;
        GenerateNextPatrolPoint();

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
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, nextPoint) < 0.25f)
        {
            GenerateNextPatrolPoint();
        }
        MoveTowards(nextPoint);
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
        anim.SetBool("isRunning", true);
        anim.SetFloat("Move X", (targetPosition.x - transform.position.x));
        anim.SetFloat("Move Y", (targetPosition.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
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

        }
    }
}
