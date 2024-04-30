using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Vector3 startPos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    public float lifetime = 5.0f;
    public float maxDistance = 100f;

    [SerializeField]
    private int damage;

    private EnemyController enemyController;
    private AgentControllerTest agentController;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();

        startPos = transform.position;

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        direction.z = 0;

        Vector2 rotation = transform.position - mousePos;

        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rotate = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotate + 180);
    }

    void Update()
    {
        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall"))
        {
            enemyController = collision.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Agent") || collision.CompareTag("Wall"))
        {
            agentController = collision.gameObject.GetComponent<AgentControllerTest>();
            if (agentController != null)
            {
                agentController.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}